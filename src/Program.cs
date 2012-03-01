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
        static int Main(string[] args)
        {
            Console.WriteLine("\nRandomFile v1.04 | http://www.vurdalakov.net\nThis program generates a file with given length and random content\n");

            if (args.Length < 2)
            {
                return Help();
            }

            long ticks = DateTime.Now.Ticks;

            String fileName = args[0];

            String sizeAsString = args[1];

            int seed = (args.Length > 2) && !IsOption(args[2]) ? Convert.ToInt32(args[2]) : -1;
            RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator(seed);

            UInt64 fileSize;
            if (sizeAsString.EndsWith("K", StringComparison.CurrentCultureIgnoreCase) || sizeAsString.EndsWith("KB", StringComparison.CurrentCultureIgnoreCase))
            {
                fileSize = Convert.ToUInt64(sizeAsString.Substring(0, sizeAsString.Length - 1)) * 1024;
            }
            else if (sizeAsString.EndsWith("M", StringComparison.CurrentCultureIgnoreCase) || sizeAsString.EndsWith("MB", StringComparison.CurrentCultureIgnoreCase))
            {
                fileSize = Convert.ToUInt64(sizeAsString.Substring(0, sizeAsString.Length - 1)) * 1024 * 1024;
            }
            else if (sizeAsString.EndsWith("G", StringComparison.CurrentCultureIgnoreCase) || sizeAsString.EndsWith("GB", StringComparison.CurrentCultureIgnoreCase))
            {
                fileSize = Convert.ToUInt64(sizeAsString.Substring(0, sizeAsString.Length - 1)) * 1024 * 1024 * 1024;
            }
            else
            {
                fileSize = Convert.ToUInt64(sizeAsString);
            }

            HashCalculator hashCalculator = new HashCalculator();

            for (int i = 1; i < args.Length; i++)
            {
                if (IsOption(args[i]))
                {
                    switch (args[i].Substring(1).ToLower())
                    {
                        case "sha1":
                            hashCalculator.CalculateSha1Hash = true;
                            break;
                        case "ascii":
                            randomNumberGenerator.AsciiCharactersOnly = true;
                            break;
                        default:
                            return Help();
                    }
                }
            }

            Console.WriteLine("Generating '{0}' file of size {1:N0} bytes using seed {2}", fileName, fileSize, randomNumberGenerator.Seed);

            int previousPercent = -1;

            UInt32 bufferSize = 65536;

            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 65536, FileOptions.SequentialScan | FileOptions.WriteThrough))
            {
                byte[] buffer = new byte[bufferSize];

                UInt64 numberOfFullBuffers = fileSize / (UInt64)buffer.Length;
                for (UInt64 i = 0; i < numberOfFullBuffers; i++)
                {
                    randomNumberGenerator.NextBytes(buffer);
                    hashCalculator.TransformBlock(buffer);
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
                    randomNumberGenerator.NextBytes(buffer);
                    hashCalculator.TransformBlock(buffer); 
                    fileStream.Write(buffer, 0, buffer.Length);
                }

                hashCalculator.TransformFinalBlock();
            }

            if (hashCalculator.CalculateSha1Hash)
            {
                Console.WriteLine("SHA-1 hash: {0}", hashCalculator.Sha1Hash);
            }

            Console.WriteLine("\rGenerated in {0:N1} seconds", Convert.ToDouble(DateTime.Now.Ticks - ticks) / 10000000);

            return 0;
        }

        private static Boolean IsOption(String text)
        {
            return ('-' == text[0]) || ('/' == text[0]);
        }

        private static int Help()
        {
            Console.WriteLine("Usage:\t\trandomfile <file name> <file size> [seed] [/options]");
            Console.WriteLine("\nSize:\t\t- add K or KB for kilobytes (32K or 256KB)\n\t\t- add M or MB for megabytes (16M or 100MB)\n\t\t- add G or GB for gigabytes (2G or 16GB)");
            Console.WriteLine("\nOptions:\t/ascii\t- generate only ASCII characters (32-127)\n\t\t/sha1\t- calculate SHA-1 hash");
            Console.WriteLine("\nExamples:\trandomfile random.bin 1073741824\n\t\trandomfile random.bin 1048576K /sha1\n\t\trandomfile random.bin 1024M\n\t\trandomfile random.bin 1G\n\t\trandomfile random.bin 64kb 12345 /ascii\n\t\trandomfile random.bin 8mb 67890");
            
            return 1;
        }
    }
}
