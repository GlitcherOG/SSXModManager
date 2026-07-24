using System;
using System.Collections.Generic;
using System.Text;

namespace SSXModManagerWinForm
{
    internal class PS2CDReading
    {
        public static void Extract(string binPath, string OutputPath)
        {
            const int RawSectorSize = 2352;
            const int UserDataOffset = 24;
            const int UserDataSize = 2048;

            using var input = File.OpenRead(binPath);
            using var output = File.Create(OutputPath);

            byte[] sector = new byte[RawSectorSize];

            while (input.Read(sector, 0, RawSectorSize) == RawSectorSize)
            {
                output.Write(sector, UserDataOffset, UserDataSize);
            }
        }
    }
}
