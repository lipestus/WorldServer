using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModels.DataStructs
{
    public class SerializableGridBounds : IDarkRiftSerializable
    {
        public int MinX { get; set; }
        public int MinY { get; set; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public void Deserialize(DeserializeEvent e)
        {
            MinX = e.Reader.ReadInt32();
            MinY = e.Reader.ReadInt32();
            MaxX = e.Reader.ReadInt32();
            MaxY = e.Reader.ReadInt32();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(MinX);
            e.Writer.Write(MinY);
            e.Writer.Write(MaxX);
            e.Writer.Write(MaxY);
        }
    }
}
