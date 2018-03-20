using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioAlgo;

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
            int dist = PairwiseSequenceAlignment.CountHammingDistance(a, b);
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
                dist = PairwiseSequenceAlignment.CountHammingDistance(seqs[0], seqs[i]);
                Assert.AreEqual(expected[i], dist);
            }
        }

        [TestMethod]
        public void CountEditDistance_SameString_ReturnsZero()
        {
            string a = "red";
            int dist = PairwiseSequenceAlignment.CountEditDistance(a, a);
            Assert.AreEqual(0, dist);
        }

        [TestMethod]
        public void CountEditDistance_OneReplacement_ReturnsOne()
        {
            string a = "doll";
            string b = "poll";
            int dist = PairwiseSequenceAlignment.CountEditDistance(a, b);
            Assert.AreEqual(1, dist);
        }

        [TestMethod]
        public void CountEdistDistance_OneDeletion_ReturnsOne()
        {
            string a = "anna";
            string b = "ann";
            int dist = PairwiseSequenceAlignment.CountEditDistance(a, b);
            Assert.AreEqual(1, dist);
        }

        [TestMethod]
        public void CountEditDistance_OneInsertion_ReturnsOne()
        {
            string a = "les";
            string b = "ples";
            int dist = PairwiseSequenceAlignment.CountEditDistance(a, b);
            Assert.AreEqual(1, dist);
        }
    }
}
