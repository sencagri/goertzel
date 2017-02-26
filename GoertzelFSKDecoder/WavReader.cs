using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoertzelFSKDecoder
{
    class WavReader
    {
        public string FilePath { get; set; }

        public byte[] ReadFile()
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

                    if (sonuç == null)
                    {
                        sonuç = new byte[0];
                    }
                    return sonuç;
                }
            }
            catch (Exception)
            {
                throw new Exception("Belirtilen dosya okunamadı!");
            }

        }
    }
}
