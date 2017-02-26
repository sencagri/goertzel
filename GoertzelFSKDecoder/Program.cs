﻿using System;
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

            wr.FilePath = args[0];
            byte[] fileData = null;

            if (!string.IsNullOrEmpty(wr.FilePath))
            {
                fileData = wr.ReadFile();
            }

            // trim all the zeros from data
            fileData  = fileData.ProcessArray();

        }
    }
}
