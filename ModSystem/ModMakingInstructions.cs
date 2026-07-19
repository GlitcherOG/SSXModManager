using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SSXModManagerWinForm.ModSystem
{
    public class ModMakingInstructions
    {
        public List<Instruction> Instructions = new List<Instruction>();

        public void Save(string paths)
        {
            string Main = "";
            for (int i = 0; i < Instructions.Count; i++)
            {
                Main += Instructions[i].Type + "," + Instructions[i].Source + "," + Instructions[i].Ouput;
                if(i!= Instructions.Count-1)
                {
                    Main += Environment.NewLine;
                }
            }
            File.WriteAllText(paths, Main);
        }

        public void Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                Instructions = new List<Instruction>();
                string[] Array = File.ReadAllLines(paths);
                for (int i = 0; i < Array.Length; i++)
                {
                    Instruction instruction = new Instruction();
                    string[] SplitLine = Array[i].Split(',');
                    instruction.Type = SplitLine[0].ToLower();
                    instruction.Source = SplitLine[1].ToLower();
                    instruction.Ouput = SplitLine[2].ToLower();
                    Instructions.Add(instruction);
                }
            }
            else
            {
                Instructions = new List<Instruction>();
            }
        }

        public void LoadText(string Text)
        {
            Instructions = new List<Instruction>();
            string[] Array = Text.Replace("\r", "").Split("\n");
            for (int i = 0; i < Array.Length; i++)
            {
                Instruction instruction = new Instruction();
                string[] SplitLine = Array[i].Split(',');
                instruction.Type = SplitLine[0].ToLower();
                instruction.Source = SplitLine[1].ToLower();
                instruction.Ouput = SplitLine[2].ToLower();
                Instructions.Add(instruction);
            }
        }
    }

    public struct Instruction
    {
        public string Type;
        public string Source;
        public string Ouput;
    }
}
