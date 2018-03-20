using System;
using static System.Console;

namespace BioAlgo
{
    public class SequenceAlignment
    {
        static int[,] editDists;
        static string[] lastSeqs = new string[2];

        // Count the number of positions in which two strings differ
        // Return -1 when two strings have different length
        static public int CountHammingDistance (string a, string b)
        {
            int dist = 0;
            if (a.Length != b.Length)
                return -1;
            
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    ++dist;
            }
            return dist;
        }

        // Count the minimum number of operations needed to turn string a into string b.
        // By edit operations we understand inserting a character into a sequence, deletion of a character from a sequence and a character replacement. 
        // We assume that both strings are not empty or null.
        static public int CountEditDistance (string a, string b)
        {
            int firstLen = a.Length + 1;
            int secondLen = b.Length + 1;
            lastSeqs = new string[]{ a, b };

            editDists = new int[firstLen, secondLen];

            for (int i = 0; i < firstLen; i++)
                editDists[i, 0] = i;
            for (int j = 0; j < secondLen; j++)
                editDists[0, j] = j;

            int delta(int i, int j) => a[i - 1] == b[j - 1] ? 0 : 1;           

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
            {
                for (int j = 1; j < secondLen; j++)
                {
                    editDists[i, j] = findMinimum(i, j);
                }
            }
            return editDists[a.Length - 1, b.Length - 1];
        }

        //// Returns all alignments of two strings using matrix of edit distances
        //public void FindAllAlignmentsUsingEditDistance(string a, string b)
        //{
        //    if (!(a.Equals(lastSeqs[0]) && b.Equals(lastSeqs[1])))
        //        CountEditDistance(a, b);

        //    int i = editDists.GetLength(0), j = editDists.GetLength(1);
        //    while (i != 0 && j != 0)
        //    {
        //        edi
        //    }
        //}
    }
}
