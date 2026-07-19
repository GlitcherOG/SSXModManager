using System;
using System.Collections.Generic;
using System.Text;

namespace SSXModManagerWinForm.ModSystem
{
    internal class GameSettings
    {
        public static void GenerateStandardSettings(string path)
        {
            List<string> Lines = new List<string>();
            Lines.Add("[EmuCore]");
            Lines.Add("HostFs = true");
            Lines.Add("");
            Lines.Add("[EmuCore/GS]");
            Lines.Add("accurate_blending_unit = 3");
            Lines.Add("");
            Lines.Add("[EmuCore/CPU]");
            Lines.Add("FPU.Roundmode = 0");
            File.WriteAllLines(path, Lines);
        }
    }
}
