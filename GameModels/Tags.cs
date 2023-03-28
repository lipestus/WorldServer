
namespace GameModels
{
    public class Tags
    {
        public enum MessageTypes : ushort
        {
            // Player related
            SpawnPlayer = 30,
            RequestSpawnPlayer = 31,
            PlayerConnected = 32,
            PlayerMovement = 33,
            PlayerInput = 34,
        }
    }
}
