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
        // Parses descriptions and sequences of molecules from FASTA file.
        // We suppose description of molecule begins with '>' followed by any characters.
        // A sequence can contain any letter of alphabet. There can be empty line.
        // Anything different is considered an error.
        // Every sequence is stored uppercase.
        // Returns null for an empty file.
        public static Dictionary<string, string> Parse(StreamReader fastaFile)
        {
            Dictionary<string, string> molecules = new Dictionary<string, string>();
            string description = "";
            string sequence = "";
            string line;
            while ((line = fastaFile.ReadLine()) != null)
            {
                if (line.Length == 0)
                    continue;
                Match matchName = Regex.Match(line, @"\>(.*)");
                if (matchName.Success)
                {
                    if (!description.Equals(""))
                    {
                        molecules.Add(description, sequence.ToUpper());
                        description = "";
                        sequence = "";
                    }
                    description = matchName.Groups[1].Value;
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
                    else throw new InvalidDataException("Input contains forbidden character(s)"); 
                }
            }
            molecules.Add(description, sequence);
            if (description.Equals("") && sequence.Equals(""))
                return null;
            else
                return molecules;
        }
    }
}
