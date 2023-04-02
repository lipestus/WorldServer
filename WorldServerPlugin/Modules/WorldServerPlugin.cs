using System;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Server;
using GameModels;
using GameModels.Models;
using System.Threading;
using WorldServerPlugin.Game;

namespace WorldServerPlugin
{
    public class WorldServerPlugin : Plugin
    {
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public string WorldServerName { get; set; }

        private PlayerManager playerManager;
        private PlayerSpawner playerSpawner;
        private AOIManager aoiManager;

        private Game.Game game;

        public WorldServerPlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
            WorldServerName = pluginLoadData.Settings["WorldServerName"] ?? "DefaultWorldServerName";
            game = new Game.Game();
            playerManager = new PlayerManager(BroadcastMessageToAllClients, game.AOIManager);
            playerSpawner = new PlayerSpawner(playerManager, BroadcastMessageToAllClients);
            InitialiseListeners();

            
            _ = game.Run();
        }

        private void InitialiseListeners()
        {
            ClientManager.ClientConnected += ClientManager_ClientConnected;
            ClientManager.ClientDisconnected += ClientManager_ClientDisconnected;
            Console.WriteLine($"{WorldServerName} * World Server is listening...");
        }

        private void ClientManager_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            int playerId = e.Client.ID;

            // Log the player ID
            Console.WriteLine($"Player {playerId} connected to the world...");

            e.Client.MessageReceived += OnMessageReceived;
            game.AddClient(e.Client);
        }
        private void ClientManager_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            e.Client.MessageReceived -= OnMessageReceived;
            var playerId = e.Client.ID;

            // Log the player ID
            Console.WriteLine($"Player {playerId} disconnected from the world...");

            DespawnPlayerMessage despawnPlayerMessage = new DespawnPlayerMessage
            {
                PlayerID = playerId,
            };

            using Message message = Message.Create((ushort)Tags.MessageTypes.DespawnPlayer, despawnPlayerMessage);
            foreach (var client in ClientManager.GetAllClients())
            {
                if (client.ID != playerId)
                {
                    client.SendMessage(message, SendMode.Reliable);
                }
            }

            playerSpawner.DespawnPlayer(playerId);
            game.RemoveClient(e.Client);
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            using (Message message = e.GetMessage())
            {
                if (message.Tag == (ushort)Tags.MessageTypes.RequestSpawnPlayer)
                {
                    var requestSpawnPlayerMessage = message.Deserialize<RequestSpawnPlayerMessage>();
                    int playerId = requestSpawnPlayerMessage.PlayerID;
                    Console.WriteLine($"Player {playerId} requested spawn...");

                    Player newPlayer = new Player(playerId, 0f, 0f);

                    // Spawn the player using the PlayerSpawner
                    playerSpawner.SpawnPlayer(playerId, newPlayer);
                }

                if (message.Tag == (ushort)Tags.MessageTypes.PlayerInput)
                {
                    PlayerInputMessage playerInputMessage = message.Deserialize<PlayerInputMessage>();

                    // Update the player's position using the PlayerManager
                    playerManager.HandlePlayerInput(playerInputMessage.PlayerID, playerInputMessage.Horizontal, playerInputMessage.Vertical, playerInputMessage.DeltaTime);

                    // Log the player input update
                    Console.WriteLine($"Player {playerInputMessage.PlayerID} input: Horizontal ({playerInputMessage.Horizontal}), Vertical ({playerInputMessage.Vertical}), DeltaTime ({playerInputMessage.DeltaTime})");
                }
            }
        }

        private void BroadcastMessageToAllClients(Message message)
        {
            foreach (IClient client in ClientManager.GetAllClients())
            {
                if (client.ConnectionState == ConnectionState.Connected)
                {
                    client.SendMessage(message, SendMode.Reliable);
                }
            }
        }
    }
}
