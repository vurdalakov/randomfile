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
using System.Security.Cryptography;
using System.Text;

namespace vurdalakov.randomfile
{
    public class HashCalculator
    {
        public Boolean CalculateSha1Hash
        {
            get
            {
                return hashAlgorithmSha1 != null;
            }
            set
            {
                hashAlgorithmSha1 = value ? HashAlgorithm.Create("SHA1") : null;
            }
        }

        private HashAlgorithm hashAlgorithmSha1 = null;

        public void TransformBlock(byte[] buffer)
        {
            if (hashAlgorithmSha1 != null)
            {
                hashAlgorithmSha1.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
            }
        }

        public void TransformFinalBlock()
        {
            if (hashAlgorithmSha1 != null)
            {
                hashAlgorithmSha1.TransformFinalBlock(new byte[0], 0, 0);
            }
        }

        public String Sha1Hash { get { return null == hashAlgorithmSha1 ? String.Empty : BytesToString(hashAlgorithmSha1.Hash); } }

        private String BytesToString(Byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                stringBuilder.AppendFormat("{0:X2}", bytes[i]);
            }

            return stringBuilder.ToString();
        }
    }
}
