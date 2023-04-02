
namespace GameModels
{
    public class Tags
    {
        public enum MessageTypes : ushort
        {
            // Player related
            SpawnPlayer = 30,
            DespawnPlayer = 31,
            RequestSpawnPlayer = 32,
            PlayerConnected = 33,
            PlayerMovement = 34,
            PlayerInput = 35,

            //AOI grid data
            AOIGridData = 40,
            AOIGridDataAndBounds = 41,
        }
    }
}
