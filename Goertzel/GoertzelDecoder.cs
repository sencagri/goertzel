using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoertzelFSKDecoder
{
    public class GoertzelDecoder
    {
        public List<double> Sample { get; set; }
        public int SampleRate { get; set; }
        public List<int> TargetFreqs { get; set; }
        private List<GoertzelHelper> GoertzelHelpers { get; set; }
        public List<double> FreqPowerResult { get; set; }

        private Dictionary<int, int> TargetFreqsDict = new Dictionary<int, int>();

        public bool Finished = false;

        // internal var.s for calculation
        private int sampleCount = int.MinValue;
        private int targetFreqCount = int.MinValue;
        private double Q0 = 0;
        private double Q1 = 0;
        private double Q2 = 0;


        public GoertzelDecoder()
        {
            GoertzelHelpers = new List<GoertzelHelper>();
            Sample = new List<double>();
            TargetFreqs = new List<int>();
            FreqPowerResult = new List<double>();
        }

        private void CalculateInternalVars()
        {
            // calculate sample count 
            sampleCount = Sample.Count;
            targetFreqCount = TargetFreqs.Count;
        }

        public void RunGoertzel()
        {
            CalculateInternalVars();
            GoertzelHelpers.Clear();
            TargetFreqsDict.Clear();

            // add all frequencies to dictionary and calculate nessesary constans to prevent continious calculation of constants
            for (int i = 0; i < TargetFreqs.Count; i++)
            {
                if (!TargetFreqsDict.ContainsKey(i))
                {
                    TargetFreqsDict.Add(i, TargetFreqs[i]);
                }

                if (GoertzelHelpers.Count <= targetFreqCount)
                {
                    GoertzelHelpers.Add(new GoertzelHelper(SampleRate, sampleCount, TargetFreqsDict[i]));
                }
            }

            // check if dict elements is missing or not
            if (TargetFreqsDict.Count == 0)
            {
                throw new Exception("Cannot run goertzel. Be sure that you have set sample rate, target frequencies properties");
            }
        }

        public void DecodeGoertzel()
        {
            Finished = false;

            // calculate sampleCount 
            CalculateInternalVars();

            // for every target freq run decoding algo. for it
            for (int i = 0; i < targetFreqCount; i++)
            {

                var helper = GoertzelHelpers[i];
                for (int j = 0; j < sampleCount; j++)
                {

                    Q0 = helper.coeff * Q1 - Q2 + Sample[j];
                    Q2 = Q1;
                    Q1 = Q0;
                }

                CalculatePower(i, helper);
                ResetGoertzel();
            }
            FreqPowerResult.Clear();
            Finished = true;
        }

        Dictionary<int, double> list = new Dictionary<int, double>();
        private void CalculatePower(int i, GoertzelHelper helper)
        {
            // calculate the power of targer freq
            var real = Math.Pow((Q1 - Q2 * helper.cosine), 2);
            var imag = Math.Pow(Q2 * helper.sine, 2);
            var mag = Math.Sqrt(real + imag);

            if (!list.ContainsKey(helper.targetFreq))
            {
                list.Add(helper.targetFreq, mag);
            }

            Debug.WriteLine("targetFreq : " + helper.targetFreq + " sampleCount : " + sampleCount + "  mag : " + +mag);

            // set result to the freqpowerres
            FreqPowerResult.Add(mag);
        }
        /// <summary>
        /// Her bir örnek için döngü tamamlandığında hesaplanan değerleri sıfırla.
        /// </summary>
        private void ResetGoertzel()
        {
            Q0 = 0;
            Q1 = 0;
            Q2 = 0;
        }
    }
}