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
        public Dictionary<char, int> index;
        public int[,] mat;

        public Matrix(String path)
        {
            StreamReader file = new StreamReader(path);
            index = new Dictionary<char, int>();
            string line = file.ReadLine();
            string[] names = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            n = names.Length;
            for (int i = 0; i < n; i++)
            {
                index.Add(names[i][0], i);
            }
            mat = new int[n, n];
            int row = 0;
            while ((line = file.ReadLine()) != null)
            {
                string[] values = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < n; i++)
                {
                    mat[i, row] = int.Parse(values[i]);
                }
                row++;
            }
            file.Close();
        }

        public int Value(char a, char b)
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
        public List<Tuple<string, string>> sequences;

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
            => position < sequences.Count ?
                sequences[position].Item2 :
                throw new ArgumentOutOfRangeException($"No sequence {position}, MSA contains only {sequences.Count} sequences.");

        public string GetSequence(string ID)
        {
            for (int i = 0; i < sequences.Count; i++)
                if (sequences[i].Item1.Equals(ID))
                    return sequences[i].Item2;
            return null;
        }

        public int GetLength()
        {
            if (sequences.Count == 0) return 0;
            return sequences[0].Item2.Length;
        }

        public char[] GetColumn(int number)
            => GetLength() > number ?
                sequences.Select(x => x.Item2[number]).ToArray() :
                throw new ArgumentOutOfRangeException($"No column {number}, MSA contains only {GetLength()}.");

        private int GetColumnScore(Matrix m, int gap, int column)
        {
            int score = 0;
            foreach (Tuple<string, string> seq1 in sequences)
            {
                foreach (Tuple<string, string> seq2 in sequences)
                {
                    char c1 = seq1.Item2[column];
                    char c2 = seq2.Item2[column];
                    if (c1 == c2)
                        continue;
                    else if (c1 == '-' || c2 == '-')
                        score += gap;
                    else
                        score += m.Value(c1, c2);
                }
            }
            return score;
        }

        public int[] GetRangeScores(Matrix m, int gap, int first_column, int num_columns)
        {
            if (first_column + num_columns > GetLength())
            {
                throw new ArgumentOutOfRangeException(
                    $"Cannot compute scores for range {first_column} to {first_column + num_columns - 1} " +
                    $"since MSA contains only {num_columns}.");
            }
            int[] scores = new int[num_columns];
            for (int i = 0; i < num_columns; i++)
            {
                scores[i] = GetColumnScore(m, gap, first_column + i);
            }
            return scores;
        }

        // Compute sum of pairs conservation score using substitution matrix m.
        public int[] GetScores(Matrix m, int gap)
        {
            return GetRangeScores(m, gap, 0, GetLength());
        }

        public List<Tuple<int,int>> TopScores(int[] scores, int k)
        {
            List<Tuple<int, int>> s = new List<Tuple<int, int>>();
            for (int i = 0; i < scores.Length; i++)
                s.Add(new Tuple<int, int>(i, scores[i]));
            s.Sort((a, b) => b.Item2.CompareTo(a.Item2));
            return s.GetRange(0, k);
        }
    }
}