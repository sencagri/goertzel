using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoertzelFSKDecoder
{
    public class GoertzelHelper
    {
        public int k = int.MinValue;
        public double omega = float.MinValue;
        public double cosine = float.MinValue;
        public double sine = float.MinValue;
        public double coeff = float.MinValue;

        public GoertzelHelper(int sampleRate, int sampleCount, int targetFreq)
        {
            k = (int)(0.5 + sampleCount * (double)targetFreq / sampleRate);
            omega = (float)(2 * Math.PI * k / sampleCount);
            sine = Math.Sin(omega);
            cosine = Math.Cos(omega);
            coeff = 2 * cosine;
        }
    }

}
}
