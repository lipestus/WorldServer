using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModels.Models
{
    public class PlayerSpawnMessage : IDarkRiftSerializable
    {
        public int PlayerID { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public void Deserialize(DeserializeEvent e)
        {
            PlayerID = e.Reader.ReadInt32();
            PositionX = e.Reader.ReadSingle();
            PositionY = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(PlayerID);
            e.Writer.Write(PositionX);
            e.Writer.Write(PositionY);
        }
    }
}
