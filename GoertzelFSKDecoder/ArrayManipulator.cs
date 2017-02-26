using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoertzelFSKDecoder
{
    public static class ArrayManipulator
    {
        private const int zeroObserver = 10;
        private const int valueObserver = 10;

        private static int start = 0;
        private static int end = 0;

        public static byte[] ProcessArray(this byte[] byteArray)
        {
            FindTheStartOfByteArray(byteArray);
            FindTheEndOfByteArray(byteArray, start);
            var result = ArrayCopyFromIndex(byteArray, start, end) ?? new byte[0];

            return result;
        }

        public static void FindTheStartOfByteArray(this byte[] byteArray, int startIndex = 0)
        {
            var resultIndex = -1;
            var byteArrayLength = byteArray.Length;
            for (int i = 0; i < byteArrayLength; i++)
            {
                // eğer sıfır harici değer tespit edilirse orada inceleme yap
                if (byteArray[i] != 0)
                {
                    // kendisinden sonra 2 tane daha var mı kontrol et
                    if (i + 2 <= byteArrayLength)
                    {
                        // tüm şartlar sağlandı ise startIndex i ye eşit olur
                        startIndex = i;

                        // eğer kendinden sonraki 2 tane de sıfır ise pas geç ve bir sonraki değeri i+3 yap
                        if (byteArray[i + 1] == 0 && byteArray[i + 2] == 0)
                        {
                            var pos = startIndex;
                            var zeros = 0;

                            for (int j = startIndex + 1; j <= startIndex + zeroObserver + 1; j++)
                            {
                                if (byteArray[j] == 0)
                                {
                                    zeros++;
                                }
                                else
                                {
                                    pos = j;
                                }
                            }
                            if (zeros == zeroObserver)
                            {
                                i = startIndex + zeroObserver;
                            }
                            else
                            {
                                resultIndex = pos;
                            }
                        }
                        else
                        {
                            resultIndex = i;
                        }
                    }
                    resultIndex = i;
                }
                if (resultIndex != -1)
                {
                    start = resultIndex;
                    break;
                }
            }
        }

        private static void FindTheEndOfByteArray(this byte[] byteArray, int startIndex = 0)
        {
            var resultIndex = -1;
            var byteArrayLength = byteArray.Length;
            for (int i = startIndex; i < byteArrayLength; i++)
            {
                startIndex = 0;
                // eğer sıfır tespit edilirse orada inceleme yap
                if (byteArray[i] == 0)
                {
                    startIndex = i;
                    var pos = startIndex;
                    var values = 0;

                    for (int j = startIndex + 1; j <= startIndex + valueObserver + 1; j++)
                    {
                        if (byteArray[j] != 0)
                        {
                            values++;
                        }
                    }
                    if (values != 0)
                    {
                        i = startIndex + valueObserver;
                    }
                    else
                    {
                        resultIndex = pos;
                    }
                }
                if (resultIndex != -1)
                {
                    end = resultIndex;

                    break;
                }
            }
        }

        private static byte[] ArrayCopyFromIndex(byte[] byteArray, int startIndex, int stopIndex)
        {
            List<byte> sonuç = new List<byte>();
            for (int i = startIndex; i < stopIndex; i++)
            {
                sonuç.Add(byteArray[i]);
            }
            return sonuç.ToArray();
        }

    }
}







