using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoertzelFSKDecoder
{
    class WavReader
    {
        public string FilePath { get; set; }
        public int SampleRate { get; set; }
        public double[] ReadFile()
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(FilePath, FileMode.Open)))
                {
                    int chunkID = reader.ReadInt32();
                    int fileSize = reader.ReadInt32();
                    int riffType = reader.ReadInt32();
                    int fmtID = reader.ReadInt32();
                    int fmtSize = reader.ReadInt32();
                    int fmtCode = reader.ReadInt16();
                    int channels = reader.ReadInt16();
                    int sampleRate = reader.ReadInt32();
                    int fmtAvgBPS = reader.ReadInt32();
                    int fmtBlockAlign = reader.ReadInt16();
                    int bitDepth = reader.ReadInt16();

                    if (fmtSize == 18)
                    {
                        // Read any extra values
                        int fmtExtraSize = reader.ReadInt16();
                        reader.ReadBytes(fmtExtraSize);
                    }

                    int dataID = reader.ReadInt32();
                    int dataSize = reader.ReadInt32();
                    var sonuç = reader.ReadBytes(dataSize);
                    var sonuçPcm = new double[sonuç.Length/2];
                    for (int i = 0; i < dataSize/2; i++)
                    {
                        var sampleValue = BitConverter.ToInt16(sonuç, i*2);
                        sonuçPcm[i] = sampleValue/32768.0;
                    }


                    if (sonuçPcm == null)
                    {
                        sonuç = new byte[0];
                    }

                    SampleRate = sampleRate;
                    return sonuçPcm;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
