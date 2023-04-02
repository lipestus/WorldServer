
namespace WorldServerPlugin
{
    public class Player : Entity
    {
        public int PlayerID { get; set; }

        // Add other properties needed for your game, such as health, inventory, etc.

        public Player(int playerId, float positionX, float positionY) : base(positionX, positionY)
        {
            PlayerID = playerId;
        }
    }
}
