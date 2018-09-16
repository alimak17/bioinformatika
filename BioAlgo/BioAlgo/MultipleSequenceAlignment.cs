using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BioAlgo
{
    public class Matrix
    {
        public int n;
        public Dictionary<string, int> index;
        public int[,] mat;

        public Matrix(String path)
        {
            StreamReader file = new StreamReader(path);
            index = new Dictionary<string, int>();
            string line = file.ReadLine();
            string[] names = line.Split((char []) null, StringSplitOptions.RemoveEmptyEntries);
            n = names.Length;
            for (int i = 0; i < n; i++)
            {
                index.Add(names[i], i);
            }
            mat = new int[n, n];
            int row = 0;
            while ((line = file.ReadLine()) != null)
            {
                string[] values = line.Split((char []) null, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < n; i++)
                {
                    mat[i, row] = int.Parse(values[i]);
                }
                row++;                
            }
            file.Close();
        }

        public int Value(string a, string b)
        {
            return mat[index[a], index[b]];
        }

        public override string ToString()
        {
            string ret = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    ret += mat[i, j] + ", ";
                }
                ret += "\n";
            }
            return ret;
        }
    }
    // Class used for multiple sequence alignment
    public class MSA
    {
        private List<Tuple<string, string>> sequences;

        public MSA(StreamReader clustalFile)
            => sequences = Parse(clustalFile);

        public MSA(List<Tuple<string, string>> seqs)
            => sequences = seqs;

        public static List<Tuple<string, string>> Parse(StreamReader clustalFile)
        {
            List<Tuple<string, string>> seqs = new List<Tuple<string, string>>();
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
            => position < sequences.Count ? sequences[position].Item2 : throw new ArgumentOutOfRangeException("MSA does not contain so many sequences.");

        public string GetSequence(string ID)
        {
            for (int i = 0; i < sequences.Count; i++)
                if (sequences[i].Item1.Equals(ID))
                    return sequences[i].Item2;
            return null;
        }

        public char[] GetColumn(int number)
            => sequences[0].Item2.Length > number ? sequences.Select(x => x.Item2[number]).ToArray() : throw new ArgumentOutOfRangeException("Sequences are not so long");

        public void GetScore(int[,] scoreMatrix)
        {
        }
    }
}