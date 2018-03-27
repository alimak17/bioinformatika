using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioAlgo;
using System.Collections.Generic;

namespace BioAlgoTests
{
    [TestClass]
    public class BioAlgoTests
    {
        [TestMethod]
        public void CountHammingDistance_DifferentLength_ReturnsMinus1()
        {
            string a = "blabla";
            string b = "patla";
            int dist = PSA.CountHammingDistance(a, b);
            Assert.AreEqual(-1, dist);
        }

        [TestMethod]
        public void CountHammingDistance_SameLength()
        {
            string[] seqs = new string[] { "abba", "baba", "cdef" };
            int[] expected = new int[] { 0, 2, 4 };
            int dist;
            for (int i = 0; i < seqs.Length; i++)
            {
                dist = PSA.CountHammingDistance(seqs[0], seqs[i]);
                Assert.AreEqual(expected[i], dist);
            }
        }

        [TestMethod]
        public void CountEditDistance_SameString_ReturnsZero()
        {
            string a = "red";
            int dist = PSA.CountEditDistance(a, a);
            Assert.AreEqual(0, dist);
        }

        [TestMethod]
        public void CountEditDistance_OneReplacement_ReturnsOne()
        {
            string a = "doll";
            string b = "poll";
            int dist = PSA.CountEditDistance(a, b);
            Assert.AreEqual(1, dist);
        }

        [TestMethod]
        public void CountEdistDistance_OneDeletion_ReturnsOne()
        {
            string a = "anna";
            string b = "ann";
            int dist = PSA.CountEditDistance(a, b);
            Assert.AreEqual(1, dist);
        }

        [TestMethod]
        public void CountEditDistance_OneInsertion_ReturnsOne()
        {
            string a = "les";
            string b = "ples";
            int dist = PSA.CountEditDistance(a, b);
            Assert.AreEqual(1, dist);
        }

        [TestMethod]
        public void FindAllAlignmentsUsingEditDistance_MoreAlignmentsUsingCountedTable_ReturnsListWithThreeElements()
        {
            PSA.editDists = new int[,] { { 0, 1, 2, 3, 4, 5, 6, 7 },
                                                               { 1, 1, 2, 3, 4, 5, 6, 7 },
                                                               { 2, 2, 2, 2, 3, 4, 5, 6 },
                                                               { 3, 3, 3, 3, 3, 4, 5, 6 },
                                                               { 4, 4, 4, 4, 3, 4, 5, 6 },
                                                               { 5, 5, 5, 5, 4, 4, 5, 6 },
                                                               { 6, 6, 6, 6, 5, 4, 5, 6 },
                                                               { 7, 7, 6, 7, 6, 5, 4, 5 }, };
            PSA.lastSeqs[0] = "WRITERS";
            PSA.lastSeqs[1] = "VINTNER";
            List<Tuple<string, string>> alignments = PSA.FindAllAlignmentsUsingEditDistance("WRITERS", "VITNERS");
            Assert.AreEqual(3, alignments.Count);
        }

        
    }
}
