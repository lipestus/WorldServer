using DarkRift;
using GameModels.DataStructs;
using System;
using System.Collections.Generic;

namespace GameModels.Models
{
    public class AOIGridDataMessage : IDarkRiftSerializable
    {
        public Dictionary<Vector2Int, List<int>> Grid { get; set; } // Use List<int> to store Entity IDs

        public void Deserialize(DeserializeEvent e)
        {
            int count = e.Reader.ReadInt32();
            Grid = new Dictionary<Vector2Int, List<int>>(count);

            for (int i = 0; i < count; i++)
            {
                Vector2Int key = e.Reader.ReadSerializable<Vector2Int>();
                int listCount = e.Reader.ReadInt32();
                List<int> value = new List<int>(listCount);

                for (int j = 0; j < listCount; j++)
                {
                    value.Add(e.Reader.ReadInt32());
                }

                Grid.Add(key, value);
            }
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(Grid.Count);

            foreach (KeyValuePair<Vector2Int, List<int>> entry in Grid)
            {
                e.Writer.Write(entry.Key);
                e.Writer.Write(entry.Value.Count);

                foreach (int entityId in entry.Value)
                {
                    e.Writer.Write(entityId);
                }
            }
        }
    }
}
