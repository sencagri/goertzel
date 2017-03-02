using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoertzelFSKDecoder
{
    public class GoertzelHelper
    {
        public double k = int.MinValue;
        public double omega = double.MinValue;
        public double cosine = double.MinValue;
        public double sine = double.MinValue;
        public double coeff = double.MinValue;
        public int targetFreq = int.MinValue;

        public GoertzelHelper(int sampleRate, int sampleCount, int targetFreq)
        {
            this.targetFreq = targetFreq;
            
            k = 0.5 + (1.0*sampleCount*targetFreq/sampleRate);
            omega = (double)(2 * Math.PI * k / sampleCount);
            sine = Math.Sin(omega);
            cosine = Math.Cos(omega);
            coeff = 2 * cosine;
            
            /*
            omega = 2*Math.PI*targetFreq*1.0/sampleRate;
            sine = Math.Sin(omega);
            cosine = Math.Cos(omega);
            coeff = 2 * cosine;
            */
        }
    }
}
