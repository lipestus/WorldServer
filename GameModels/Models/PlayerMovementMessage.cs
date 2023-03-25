using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModels.Models
{
    public class PlayerMovementMessage : IDarkRiftSerializable
    {
        public int PlayerID;
        public float PositionX;
        public float PositionY;
        public float DeltaTime;

        // Add any other properties needed for your game (e.g., animation state)

        public void Deserialize(DeserializeEvent e)
        {
            PlayerID = e.Reader.ReadInt32();
            PositionX = e.Reader.ReadSingle();
            PositionY = e.Reader.ReadSingle();
            DeltaTime = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(PlayerID);
            e.Writer.Write(PositionX);
            e.Writer.Write(PositionY);
            e.Writer.Write(DeltaTime);
        }
    }
}
