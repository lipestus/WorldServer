using DarkRift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModels.Models
{
    public class RequestSpawnPlayerMessage : IDarkRiftSerializable
    {
        public int PlayerID { get; set; }

        public void Deserialize(DeserializeEvent e)
        {
            PlayerID = e.Reader.ReadInt32();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(PlayerID);
        }
    }
}
