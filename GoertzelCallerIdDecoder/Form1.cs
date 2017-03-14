using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DevExpress.XtraEditors;
using GoertzelFSKDecoder;
using NAudio.Wave;


namespace GoertzelCallerIdDecoder
{
    public partial class Form1 : XtraForm
    {
        private BufferedWaveProvider bwp;
        private WaveInEvent wi;
        private bool firstTime = true;
        private bool isRecording = false;
        GoertzelDecoder gd = new GoertzelDecoder();

        public Form1()
        {
            InitializeComponent();

            gd.TargetFreqs.Add(100);
            gd.TargetFreqs.Add(500);
            gd.TargetFreqs.Add(1000);
            gd.TargetFreqs.Add(1500);
            gd.TargetFreqs.Add(2000);
            gd.TargetFreqs.Add(2500);
            gd.TargetFreqs.Add(3000);

            gd.RunGoertzel();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            freqPowerResultChart.Series.Clear();

            foreach (var pr in gd.TargetFreqs)
            {
                freqPowerResultChart.Series.Add(pr.ToString() + "hz");
            }
        }

        private void startStopButton_Click(object sender, EventArgs e)
        {
            if (firstTime || !isRecording)
            {
                if (firstTime)
                {
                    NaudioSettings();
                    firstTime = false;
                }
                wi.StartRecording();
                isRecording = true;
            }
            else
            {
                wi.StopRecording();
                wi.DataAvailable -= null;
                isRecording = false;
            }
        }

        private void NaudioSettings()
        {
            wi = new WaveInEvent();
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            bwp = new BufferedWaveProvider(wi.WaveFormat) { DiscardOnBufferOverflow = true };
            gd.SampleRate = wi.WaveFormat.SampleRate;
        }
        private void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);

            var bufferBytes = new byte[bwp.BufferLength];
            bwp.Read(bufferBytes, 0, 0);

            passDataAndProcess(bufferBytes);
        }

        private void passDataAndProcess(byte[] bufferBytes)
        {
            // clear processed data
            gd.Sample.Clear();

            // add new data
            for (int i = 0; i < bufferBytes.Length/2; i++)
            {
                var bitPCM = BitConverter.ToInt16(bufferBytes, i * 2);
                var value = (double)bitPCM;

                gd.Sample.Add(value);
            }

            // when everyting is done process the data
            gd.DecodeGoertzel();

            while (!gd.Finished)
            {
                
            }

            displayResultInChartControl();
        }

        private void displayResultInChartControl()
        {
            for (int i = 0; i < gd.FreqPowerResult.Count; i++)
            {
                if (double.IsNaN(gd.FreqPowerResult[i]))
                {
                    gd.FreqPowerResult[i] = 0;
                }

                

                DataPoint dp = new DataPoint();
                dp.SetValueY(gd.FreqPowerResult[i]);

                this.Invoke((MethodInvoker) delegate
                {
                    freqPowerResultChart.Series[i].Points.Clear();
                    freqPowerResultChart.Series[i].Points.Add(dp);
                });
            }
        }
    }
}
