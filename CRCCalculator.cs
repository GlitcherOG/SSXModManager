using CommunityToolkit.HighPerformance;
using System;
using System.IO;
using System.IO.Hashing;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SSXModManagerWinForm
{
    internal class CRCCalculator
    {
        public static string CalculateCRC32(Stream fileStream)
        {
            byte[] FileBytes = new byte[fileStream.Length];

            fileStream.Position = 0;

            fileStream.Read(FileBytes, 0, (int)fileStream.Length);

            uint crc = 0;

            for (int i = 0; i < FileBytes.Length; i += 4)
            {
                uint word = 0;

                if (i < FileBytes.Length) word |= FileBytes[i];
                if (i + 1 < FileBytes.Length) word |= (uint)FileBytes[i + 1] << 8;
                if (i + 2 < FileBytes.Length) word |= (uint)FileBytes[i + 2] << 16;
                if (i + 3 < FileBytes.Length) word |= (uint)FileBytes[i + 3] << 24;

                crc ^= word;
            }

            return crc.ToString("X");
        }

    }
}
