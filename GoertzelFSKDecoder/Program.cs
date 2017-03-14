using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using GoertzelFSKDecoder;

namespace GoertzelFSKDecoder
{
    class Program
    {
        BufferedWaveProvider bwp;
        private WaveInEvent wi;
        public static GoertzelDecoder gd2;

        static void Main(string[] args)
        {
            WavReader wr = new WavReader();
            var filePath = @"C:\Users\Ebubekir Çağrı ŞEN\OneDrive\Ebubekir\Projeler\Elektronik\CallerID\YardımcıProgramlarVeÖrnekDosyalar\fskgen\samples\test1200hz.wav";
            wr.FilePath = filePath;

            // wr.FilePath = args[0];
            double[] fileData = null;

            if (!string.IsNullOrEmpty(wr.FilePath))
            {
                fileData = wr.ReadFile();
            }

            // set naudio parameters and events to get data from microphone
            Program p = new Program();

            // trim all the zeros from data
            fileData = fileData.ProcessArray();

            GoertzelDecoder gd= new GoertzelDecoder();
            gd2 = gd;
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
            gd.TargetFreqs.Add(2200);
            gd.TargetFreqs.Add(2400);
            gd.TargetFreqs.Add(4000);

            for (int i = 0; i < 20; i++)
            {
                gd.Sample.Add(fileData[i]);
            }

            p.NaudioSettings();
            gd.RunGoertzel();
        }

        public void NaudioSettings()
        {
            wi = new WaveInEvent();
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.DiscardOnBufferOverflow = true;
            wi.StartRecording();
        }
        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
            for (int i = 0; i < bwp.BufferLength; i++)
            {
                gd2.Sample.Add(e.Buffer[i]);
            }
        }
    }
}
