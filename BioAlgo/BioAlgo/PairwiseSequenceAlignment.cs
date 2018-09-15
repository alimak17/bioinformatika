using System;
using System.Collections.Generic;
using static System.Console;

namespace BioAlgo
{
    public class PSA
    {
        static public int[,] editDists;
        static public string[] lastSeqs = new string[2];

        // Count the number of positions in which two strings differ
        // Return -1 when two strings have different length
        static public int CountHammingDistance(string A, string B)
        {
            int dist = 0;
            if (A.Length != B.Length)
                return -1;

            for (int i = 0; i < A.Length; i++)
                if (A[i] != B[i])
                    ++dist;
            return dist;
        }

        // Count the minimum number of operations needed to turn string a into string b.
        // By edit operations we understand inserting a character into a sequence, deletion
        // of a character from a sequence and a character replacement.
        // We assume that both strings are not empty or null.
        static public int CountEditDistance(string A, string B)
        {
            int firstLen = A.Length + 1;
            int secondLen = B.Length + 1;
            lastSeqs = new string[] { A, B };

            editDists = new int[firstLen, secondLen];

            for (int i = 0; i < firstLen; i++)
                editDists[i, 0] = i;
            for (int j = 0; j < secondLen; j++)
                editDists[0, j] = j;

            int delta(int i, int j) => A[i - 1] == B[j - 1] ? 0 : 1;

            int findMinimum(int i, int j)
            {
                int[] vals = new int[] {
                        editDists[i - 1, j] + 1,
                        editDists[i, j - 1] + 1,
                        editDists[i - 1, j - 1] + delta(i, j)};
                int min = Math.Min(vals[0], vals[1]);
                min = Math.Min(min, vals[2]);
                return min;
            }

            for (int i = 1; i < firstLen; i++)
                for (int j = 1; j < secondLen; j++)
                    editDists[i, j] = findMinimum(i, j);
            return editDists[A.Length - 1, B.Length - 1];
        }

        // Returns all alignments of two strings using matrix of edit distances
        public static List<Tuple<string, string>> FindAllAlignmentsUsingEditDistance(string A, string B)
        {
            if (!(A.Equals(lastSeqs[0]) && B.Equals(lastSeqs[1])))
                CountEditDistance(A, B);

            List<Tuple<string, string>> output = new List<Tuple<string, string>>();

            string Reverse(string s)
            {
                if (s == null) return null;
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                return new String(array);
            }

            void FindAllAlignments(int i, int j, string first, string second, ref List<Tuple<string, string>> alignments)
            {
                if (i == 0 && j == 0)
                {
                    alignments.Add(new Tuple<string, string>(Reverse(first), Reverse(second)));
                }
                else
                {
                    if (i > 0 && editDists[i - 1, j] + 1 == editDists[i, j])
                    {
                        FindAllAlignments(i - 1, j, first + A[i - 1], second + "-", ref alignments);
                    }
                    if (j > 0 && editDists[i, j - 1] + 1 == editDists[i, j])
                    {
                        FindAllAlignments(i, j - 1, first + "-", second + B[j - 1], ref alignments);
                    }
                    if (i > 0 && j > 0 && (editDists[i - 1, j - 1] + 1 == editDists[i, j] ||
                                           editDists[i - 1, j - 1] == editDists[i, j]))
                    {
                        FindAllAlignments(i - 1, j - 1, first + A[i - 1], second + B[j - 1], ref alignments);
                    }
                }
            }

            FindAllAlignments(A.Length, B.Length, "", "", ref output);
            return output;
        }
    }
}