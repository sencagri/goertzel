using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DevExpress.Data.Helpers;
using DevExpress.XtraEditors;
using Goertzel.Event;
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

            gd.TargetFreqs.Add(697);
            gd.TargetFreqs.Add(770);
            gd.TargetFreqs.Add(852);
            gd.TargetFreqs.Add(941);
            gd.TargetFreqs.Add(1209);
            gd.TargetFreqs.Add(1336);
            gd.TargetFreqs.Add(1477);
            gd.TargetFreqs.Add(1633);

            gd.OnGoertzelDecoded += GdOnOnGoertzelDecoded;

            gd.RunGoertzel();
        }

        private void GdOnOnGoertzelDecoded(object sender, DecodingEvents e)
        {
            displayResultInChartControl(e.FreqPowerResult);
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
                gd.RunGoertzel();

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

            var bufferBytes = new byte[bwp.BufferedBytes];
            bwp.Read(bufferBytes, 0, 0);

            for (int i = 0; i < e.BytesRecorded; i++)
            {
                bufferBytes[i] = e.Buffer[i];
            }

            passDataAndProcess(bufferBytes);
        }

        private void passDataAndProcess(byte[] bufferBytes)
        {
            // clear processed data
            gd.Sample.Clear();

            // add new data
            for (int i = 0; i < bufferBytes.Length / 2; i++)
            {
                var bitPCM = BitConverter.ToInt16(bufferBytes, i * 2);
                var value = (double)bitPCM;

                gd.Sample.Add(value);
            }

            // when everyting is done process the data
            gd.DecodeGoertzel();
        }

        List<DataPoint> dpList = new List<DataPoint>();

        private void displayResultInChartControl(List<double> freqPowerList)
        {
            try
            {
                for (int i = 0; i < freqPowerList.Count; i++)
                {
                    if (!double.IsNaN(freqPowerList[i]))
                    {
                        dpList.Add(new DataPoint());
                        dpList[i].SetValueXY(i, freqPowerList[i]);
                    }
                    else freqPowerList[i] = 0;

                    this.Invoke((MethodInvoker)delegate
                    {
                        var val = gd.TargetFreqs[i] + " hz";
                        if (!freqPowerResultChart.Series.IsUniqueName(val))
                        {
                            freqPowerResultChart.Series.Add(new Series(gd.TargetFreqs[i] + " hz") { IsXValueIndexed = true });
                        }
                    });

                    this.Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            freqPowerResultChart.Series[i].Points.Clear();
                            freqPowerResultChart.Series[i].Points.Add(dpList[i]);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    });

                }
                freqPowerList.Clear();
                Thread.Sleep(5);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
