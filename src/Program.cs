// This software is part of the RandomFile application.
// RandomFile is a command-line program that generates a file with given length and random content.
//
// Copyright (c) 2009 Vurdalakov
// http://www.vurdalakov.net
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;

namespace vurdalakov.randomfile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nRandomFile v1.02\nThis program generates a file with given length and random content\nhttp://www.vurdalakov.net");

            if ((args.Length < 2) || (args.Length > 3))
            {
                Console.WriteLine("\nUsage:\n\trandomfile <file name> <size in bytes> [seed]\n\trandomfile <file name> <sizeK or sizeKB in kilobytes> [seed]\n\trandomfile <file name> <sizeM or sizeMB in megabytes> [seed]\n\trandomfile <file name> <sizeG or sizeGB in gigabytes> [seed]");
                Console.WriteLine("\nExamples:\n\trandomfile random.bin 1073741824\n\trandomfile random.bin 1048576K\n\trandomfile random.bin 1024M\n\trandomfile random.bin 1G\n\trandomfile random.bin 64kb 12345\n\trandomfile random.bin 8mb 67890");
                return;
            }

            DateTime startTime = DateTime.Now;

            String fileName = args[0];

            String sizeAsString = args[1];

            int seed = args.Length > 2 ? Convert.ToInt32(args[2]) : -1;

            UInt64 fileSize;
            if (sizeAsString.EndsWith("K", StringComparison.CurrentCultureIgnoreCase))
            {
                fileSize = Convert.ToUInt64(sizeAsString.Substring(0, sizeAsString.Length - 1)) * 1024;
            }
            else if (sizeAsString.EndsWith("KB", StringComparison.CurrentCultureIgnoreCase))
            {
                fileSize = Convert.ToUInt64(sizeAsString.Substring(0, sizeAsString.Length - 2)) * 1024;
            }
            else if (sizeAsString.EndsWith("M", StringComparison.CurrentCultureIgnoreCase))
            {
                fileSize = Convert.ToUInt64(sizeAsString.Substring(0, sizeAsString.Length - 1)) * 1024 * 1024;
            }
            else if (sizeAsString.EndsWith("MB", StringComparison.CurrentCultureIgnoreCase))
            {
                fileSize = Convert.ToUInt64(sizeAsString.Substring(0, sizeAsString.Length - 2)) * 1024 * 1024;
            }
            else if (sizeAsString.EndsWith("G", StringComparison.CurrentCultureIgnoreCase))
            {
                fileSize = Convert.ToUInt64(sizeAsString.Substring(0, sizeAsString.Length - 1)) * 1024 * 1024 * 1024;
            }
            else if (sizeAsString.EndsWith("GB", StringComparison.CurrentCultureIgnoreCase))
            {
                fileSize = Convert.ToUInt64(sizeAsString.Substring(0, sizeAsString.Length - 2)) * 1024 * 1024 * 1024;
            }
            else
            {
                fileSize = Convert.ToUInt64(sizeAsString);
            }

            String seedString = seed >= 0 ? String.Format(" using seed {0}", seed) : "";
            Console.WriteLine("Generating '{0}' file of size {1:N0} bytes{2}", fileName, fileSize, seedString);

            int previousPercent = -1;

            Random random;
            if (seed >= 0)
            {
                random = new Random(seed);
            }
            else
            {
                random = new Random();
            }

            UInt32 bufferSize = 65536;

            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 65536, FileOptions.SequentialScan | FileOptions.WriteThrough))
            {
                byte[] buffer = new byte[bufferSize];

                UInt64 numberOfFullBuffers = fileSize / (UInt64)buffer.Length;
                for (UInt64 i = 0; i < numberOfFullBuffers; i++)
                {
                    random.NextBytes(buffer);
                    fileStream.Write(buffer, 0, buffer.Length);

                    int percent = (int)(i * 100 / numberOfFullBuffers);
                    if (percent > previousPercent)
                    {
                        Console.Write("\r{0}%", percent);
                        previousPercent = percent;
                    }
                }

                UInt64 remainder = fileSize % (UInt64)buffer.Length;
                if (remainder > 0)
                {
                    buffer = new byte[remainder];
                    random.NextBytes(buffer);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }

            TimeSpan duration = DateTime.Now.Subtract(startTime);

            Console.WriteLine("\rGenerated in {0:N1} seconds", Convert.ToDouble(duration.TotalMilliseconds) / 1000);
        }
    }
}
