using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModels
{
    public class Tags
    {
        public enum MessageTypes : ushort
        {
            // Player related
            PlayerConnected = 30,
            PlayerMovement = 31,
            PlayerInput = 32,
        }
    }
}
