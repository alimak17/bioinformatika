using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BioAlgo
{
    public class FastaParser
    {
        public static Dictionary<string, string> Parse(StreamReader fastaFile)
        {
            Dictionary<string, string> FastaSeqs = new Dictionary<string, string>();
            string name = "";
            string sequence = "";
            string line;
            while ((line = fastaFile.ReadLine()) != null)
            {
                if (line.Length == 0)
                    continue;
                Match matchName = Regex.Match(line, @"\>(.*)");
                if (matchName.Success)
                {
                    if (!name.Equals(""))
                    {
                        FastaSeqs.Add(name, sequence);
                        name = "";
                        sequence = "";
                    }
                    name = matchName.Groups[1].Value;
                    continue;
                }
                else
                {
                    Match matchSeq = Regex.Match(line, "[a-zA-Z]+");
                    if (matchSeq.Success)
                    {
                        sequence += line;
                        continue;
                    }
                    else throw new Exception("Incorrect input");
                }
            }
            FastaSeqs.Add(name, sequence);
            return FastaSeqs;
        }
    }
}
