using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServerPlugin
{
    public class Entity
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public Entity(float positionX, float positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}
