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
            Lines.Add("EnableCheats = true");
            Lines.Add("");
            Lines.Add("[EmuCore/GS]");
            Lines.Add("accurate_blending_unit = 4");
            Lines.Add("LoadTextureReplacements = true");
            Lines.Add("");
            Lines.Add("[EmuCore/CPU]");
            Lines.Add("FPU.Roundmode = 0");
            File.WriteAllLines(path, Lines);
        }

        public static void ClearCheats(string path)
        {
            List<string> PerSettingCheats = File.ReadAllLines(path).ToList();
            for (int i = 0; i < PerSettingCheats.Count; i++)
            {
                if (PerSettingCheats[i] == "[Cheats]")
                {
                    for (int j = i+1; j < PerSettingCheats.Count; j++)
                    {
                        if (PerSettingCheats[j]!= "\r\n")
                        {
                            PerSettingCheats.RemoveAt(j);
                            j--;
                        }
                        else
                        {
                            break;
                        }
                    }

                    break;
                }
            }

            File.Create(path);
            File.WriteAllLines(path, PerSettingCheats.ToArray());
        }

        public static void AddCheats(string path, string CheatName)
        {
            bool CheatsLineFound = false;
            List<string> PerSettingCheats = File.ReadAllLines(path).ToList();
            for (int i = 0; i < PerSettingCheats.Count; i++)
            {
                if (PerSettingCheats[i] == "[Cheats]")
                {
                    PerSettingCheats.Insert(i + 1, "Enable = " + CheatName);
                }
            }

            File.Create(path);
            File.WriteAllLines(path, PerSettingCheats.ToArray());
        }


        /* Basic Settings Idea
[EmuCore]
HostFs = true
EnableCheats = true


[EmuCore/GS]
accurate_blending_unit = 3
AspectRatio = 16:9
LoadTextureReplacements = true


[EmuCore/CPU]
FPU.Roundmode = 0


[Cheats]
Enable = Disable intro videos (ea / thx / splash)
Enable = Disable $20 payout limit
Enable = Fix Metro Slowdown
Enable = Fixed camera algorithm
Enable = SSX 3 NSTC Online Server Address
Enable = Spectral
Enable = No Speed Cap
Enable = Super UBER Replace
*/
    }
}
