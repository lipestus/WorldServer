using DarkRift;
using GameModels.Models;
using GameModels;
using System.Collections.Generic;

namespace WorldServerPlugin
{
    public class PlayerManager
    {
        private Dictionary<int, Player> players;
        public delegate void BroadcastMessageDelegate(Message message);
        private BroadcastMessageDelegate broadcastMessage;

        public PlayerManager(BroadcastMessageDelegate broadcastMessage)
        {
            players = new Dictionary<int, Player>();
            this.broadcastMessage = broadcastMessage;
        }

        public void AddPlayer(int playerId, Player player)
        {
            // Add the player to the dictionary
            players[playerId] = player;
        }

        public void RemovePlayer(int playerId)
        {
            // Remove the player from the dictionary
            players.Remove(playerId);
        }

        public void HandlePlayerInput(int playerId, float horizontal, float vertical, float deltaTime)
        {
            // Calculate the new position based on the input
            float newX = horizontal * deltaTime;
            float newY = vertical * deltaTime;

            // Update the player's position
            if (players.TryGetValue(playerId, out Player player))
            {
                player.PositionX += newX;
                player.PositionY += newY;

                // Broadcast the updated position to all clients
                BroadcastPlayerMovementMessage(player);
            }
            else
            {
                // Handle the case where the player ID is not found in the players dictionary
            }
        }

        public void BroadcastMessage(Message message)
        {
            broadcastMessage(message);
        }

        private void BroadcastPlayerMovementMessage(Player player)
        {
            using (DarkRiftWriter writer = DarkRiftWriter.Create())
            {
                PlayerMovementMessage playerMovementMessage = new PlayerMovementMessage
                {
                    PlayerID = player.PlayerID,
                    PositionX = player.PositionX,
                    PositionY = player.PositionY,
                };

                var serializeEvent = new SerializeEvent(writer);
                playerMovementMessage.Serialize(serializeEvent);

                using (Message message = Message.Create((ushort)Tags.MessageTypes.PlayerMovement, writer))
                {
                    this.broadcastMessage(message);
                }
            }
        }
    }
}
