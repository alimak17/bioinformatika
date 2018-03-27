using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Console;

namespace BioAlgo
{
    // Class used for multiple sequence alignment
    public class MSA
    {
        List<Tuple<string, string>> sequences;

        public MSA(StreamReader clustalFile)
            => sequences = Parse(clustalFile);

        public MSA(List<Tuple<string, string>> seqs)
            => sequences = seqs;

        public static List<Tuple<string, string>> Parse(StreamReader clustalFile)
        {
            List<Tuple<string,string>> seqs = new List<Tuple<string, string>>();
            string description = "";
            string sequence = "";
            string line;
            while ((line = clustalFile.ReadLine()) != null)
            {
                Match firstLine = Regex.Match(line, @"CLUSTAL.*");
                if (firstLine.Success)
                    continue;

                Match seqPart = Regex.Match(line, @"([^\s]+)\s+([a-zA-Z-]+)\s*[0-9]*");
                if (seqPart.Success)
                {
                    description = seqPart.Groups[1].Value;
                    sequence = seqPart.Groups[2].Value;
                    int foundAt = -1;
                    for (int i = 0; i < seqs.Count; i++)
                    {
                        if (seqs[i].Item1.Equals(description))
                        {
                            foundAt = i;
                            break;
                        }
                    }
                    if (foundAt != -1)
                        seqs[foundAt] = new Tuple<string, string>(description, seqs[foundAt].Item2 + sequence);
                    else
                        seqs.Add(new Tuple<string, string>(description, sequence));
                }
            }
            clustalFile.Close();
            if (seqs.Count == 0)
                return null;
            else
                return seqs;
        }

        public string GetSequence(int position)
        {
            if (position >= sequences.Count)
                return null;
            else
                return sequences[position].Item2;
        }

        public string GetSequence(string ID)
        {
            for (int i = 0; i < sequences.Count; i++)
                if (sequences[i].Item1.Equals(ID))
                    return sequences[i].Item2;
            return null;
        }

        public char[] GetColumn(int number)
        {
            char[] column = new char[sequences.Count];
            for (int i = 0; i < sequences.Count; i++)
                column[i] = sequences[i].Item2[number];
            return column;
        }

        public void GetScore(int[,] scoreMatrix)
        {

        }
    }
}
