using DarkRift;
using GameModels;
using GameModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServerPlugin
{
    public class PlayerSpawner
    {
        private List<(float, float)> spawnPoints;
        private PlayerManager.BroadcastMessageDelegate broadcastMessage;
        private PlayerManager playerManager;

        public PlayerSpawner(PlayerManager playerManager, PlayerManager.BroadcastMessageDelegate broadcastMessage)
        {
            this.playerManager = playerManager;
            this.broadcastMessage = broadcastMessage;
            InitializeSpawnPoints();
        }

        private void InitializeSpawnPoints()
        {
            spawnPoints = new List<(float, float)>
            {
                (0f, 0f),
                (1f, 1f),
                (-1f, -1f)
            };
        }

        public void SpawnPlayer(int playerId, Player player)
        {
            // Assign an initial spawn point to the player
            (float x, float y) = GetNextSpawnPoint();
            player.PositionX = x;
            player.PositionY = y;

            // Add the player to the PlayerManager
            playerManager.AddPlayer(playerId, player);

            // Notify all clients about the newly spawned player
            BroadcastSpawnPlayerMessage(player);
        }

        private (float, float) GetNextSpawnPoint()
        {
            // Get the next available spawn point
            // Example: return the first spawn point in the list
            return spawnPoints[0];
        }

        private void BroadcastSpawnPlayerMessage(Player player)
        {
            using (DarkRiftWriter writer = DarkRiftWriter.Create())
            {
                PlayerSpawnMessage playerSpawnMessage = new PlayerSpawnMessage
                {
                    PlayerID = player.PlayerID,
                    PositionX = player.PositionX,
                    PositionY = player.PositionY,
                };

                var serializeEvent = new SerializeEvent(writer);
                playerSpawnMessage.Serialize(serializeEvent);

                using (Message message = Message.Create((ushort)Tags.MessageTypes.SpawnPlayer, writer))
                {
                    playerManager.BroadcastMessage(message);
                }
            }
        }
    }
}
