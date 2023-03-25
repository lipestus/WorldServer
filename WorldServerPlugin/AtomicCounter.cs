using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorldServerPlugin
{
    public class AtomicCounter
    {
        private int value;

        public AtomicCounter(int initialValue)
        {
            value = initialValue;
        }

        public int GetNextValue()
        {
            return Interlocked.Increment(ref value);
        }
    }
}
