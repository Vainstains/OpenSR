using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenScreenRecorder
{
    public static class Lerp
    {
        public static float lerp(float a, float b, float t)
        {
            return a + (t * (b - a));
        }
    }
}
