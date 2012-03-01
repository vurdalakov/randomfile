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

namespace vurdalakov.randomfile
{
    public class RandomNumberGenerator
    {
        private Random random;

        private int seed;
        public int Seed { get { return seed; } }

        private Boolean asciiCharactersOnly = false;
        public Boolean AsciiCharactersOnly
        {
            get { return asciiCharactersOnly; }
            set { asciiCharactersOnly = value; }
        }

        public RandomNumberGenerator(int seed)
        {
            if (seed < 0)
            {
                seed = (int)(DateTime.Now.Ticks & 0x0000FFFF);
            }

            random = new Random(seed);

            this.seed = seed;
        }

        public virtual void NextBytes(byte[] buffer)
        {
            if (asciiCharactersOnly)
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = Convert.ToByte(random.Next(32, 127));
                }
            }
            else
            {
                random.NextBytes(buffer);
            }
        }
    }
}
