using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldServerPlugin
{
    public class GridBounds
    {
        public float XMin { get; set; }
        public float YMin { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public GridBounds(float xMin, float yMin, float width, float height)
        {
            XMin = xMin;
            YMin = yMin;
            Width = width;
            Height = height;
        }

        public float XMax => XMin + Width;
        public float YMax => YMin + Height;
    }
}
