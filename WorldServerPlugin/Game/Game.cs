using DarkRift;
using DarkRift.Server;
using GameModels.DataStructs;
using GameModels.Models;
using GameModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WorldServerPlugin.Game
{
    public class Game
    {
        // Other server variables, such as the list of connected clients, the AOIManager, etc.
        private List<IClient> clients;
        private float updateInterval = 0.1f; // Update interval in seconds (e.g., 10 times per second)
        private CancellationTokenSource cancellationTokenSource;
        private object clientsLock = new object();

        public AOIManager AOIManager { get; }

        //Grid
        private float gridDataUpdateInterval = 1.0f; // Update grid data every second
        private float timeSinceLastGridDataUpdate = 0.0f;

        public Game()
        {
            clients = new List<IClient>();
            AOIManager = new AOIManager(5); // Set cell size as appropriate for your game
            cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task Run()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                UpdateGameState();
                await Task.Delay(TimeSpan.FromSeconds(updateInterval), cancellationTokenSource.Token);
            }
        }

        public void AddClient(IClient client)
        {
            lock (clientsLock)
            {
                clients.Add(client);
            }
        }
        public void RemoveClient(IClient client)
        {
            lock (clientsLock)
            {
                clients.Remove(client);
            }
        }

        private float elapsedTime = 0.0f;
        private void UpdateGameState()
        {
            elapsedTime += updateInterval;

            if (elapsedTime >= 1.0f)
            {
                Console.WriteLine($"Game State updated after: {elapsedTime} second.");
                elapsedTime = 0.0f;
            }

            timeSinceLastGridDataUpdate += updateInterval;
            if (timeSinceLastGridDataUpdate >= gridDataUpdateInterval)
            {
                BroadcastAOIGridData();
                timeSinceLastGridDataUpdate = 0.0f;
            }
        }

        private void SendEntityUpdatesToClient(IClient client, HashSet<Entity> entities)
        {
            // Implement the logic to send entity updates to the client using DarkRift2
        }
        private void BroadcastAOIGridData()
        {
            Dictionary<Vector2Int, List<int>> gridData = AOIManager.GetGridDataWithEntityIDs();
            SerializableGridBounds gridBounds = AOIManager.GetSerializableGridBounds();
            AOIGridDataAndBoundsMessage gridDataAndBoundsMessage = new AOIGridDataAndBoundsMessage { Grid = gridData, Bounds = gridBounds };

            using (Message message = Message.Create((ushort)Tags.MessageTypes.AOIGridDataAndBounds, gridDataAndBoundsMessage))
            {
                foreach (IClient client in clients)
                {
                    if(client.ConnectionState == ConnectionState.Connected)
                        client.SendMessage(message, SendMode.Reliable);
                }
            }
        }

        public void Stop()
        {
            AOIManager.ClearGrid();
            cancellationTokenSource.Cancel();
        }
    }
}
