using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModels.Models
{
    public class PlayerInputMessage : IDarkRiftSerializable
    {
        public int PlayerID;
        public float Horizontal;
        public float Vertical;
        public float DeltaTime;

        public void Deserialize(DeserializeEvent e)
        {
            PlayerID = e.Reader.ReadInt32();
            Horizontal = e.Reader.ReadSingle();
            Vertical = e.Reader.ReadSingle();
            DeltaTime = e.Reader.ReadSingle();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(PlayerID);
            e.Writer.Write(Horizontal);
            e.Writer.Write(Vertical);
            e.Writer.Write(DeltaTime);
        }
    }
}
