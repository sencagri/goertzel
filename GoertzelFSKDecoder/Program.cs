using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoertzelFSKDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            WavReader wr = new WavReader();
            var filePath = @"C:\Users\Ebubekir Çağrı ŞEN\OneDrive\Ebubekir\Projeler\Elektronik\CallerID\YardımcıProgramlarVeÖrnekDosyalar\fskgen\samples\test-callerid.wav";
            wr.FilePath = filePath;

            // wr.FilePath = args[0];
            double[] fileData = null;

            if (!string.IsNullOrEmpty(wr.FilePath))
            {
                fileData = wr.ReadFile();
            }

            // trim all the zeros from data
            fileData = fileData.ProcessArray();

            GoertzelDecoder gd = new GoertzelDecoder();
            gd.SampleRate = wr.SampleRate;
            gd.TargetFreqs.Add(50);
            gd.TargetFreqs.Add(100);
            gd.TargetFreqs.Add(200);
            gd.TargetFreqs.Add(350);
            gd.TargetFreqs.Add(440);
            gd.TargetFreqs.Add(600);
            gd.TargetFreqs.Add(700);
            gd.TargetFreqs.Add(880);
            gd.TargetFreqs.Add(1000);
            gd.TargetFreqs.Add(1200);

            for (int i = 0; i < 200; i++)
            {
                gd.Sample.Add(fileData[i]);
            }

            gd.RunGoertzel();
        }
    }
}
