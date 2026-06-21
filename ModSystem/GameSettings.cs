using System;
using System.Collections.Generic;
using System.Text;

namespace SSXModManagerWinForm.ModSystem
{
    internal class GameSettings
    {
        public void GenerateStandardSettings(string path)
        {
            File.WriteAllText(path, "        [EmuCore]\r\n        HostFs = true");
        }
    }
}
