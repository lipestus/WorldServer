
namespace WorldServerPlugin
{
    public class Player
    {
        public int PlayerID { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }

        // Add other properties needed for your game, such as health, inventory, etc.

        public Player(int playerId, float positionX, float positionY)
        {
            PlayerID = playerId;
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}
