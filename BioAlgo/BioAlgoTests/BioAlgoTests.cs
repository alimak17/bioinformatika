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

        [TestMethod]
        public void GetSequenceByPosition_EmptyList_ReturnsNull()
        {
            List<Tuple<string, string>> seqs = new List<Tuple<string, string>>();

            MSA msa = new MSA(seqs);
            string s = msa.GetSequence(0);
            Assert.AreEqual(null, s);
        }

        [TestMethod]
        public void GetSequenceByPosition_ChooseZeroPositonThreeSequences_ReturnsSequenceOnZeroPosition()
        {
            List<Tuple<string, string>> seqs = new List<Tuple<string, string>>();
            seqs.Add(new Tuple<string, string>("als_1", "BBBC"));
            seqs.Add(new Tuple<string, string>("als_2", "AABB"));
            seqs.Add(new Tuple<string, string>("als_3", "CACA"));

            MSA msa = new MSA(seqs);
            string s = msa.GetSequence(0);
            Assert.AreEqual("BBBC", s);
        }

        [TestMethod]
        public void GetSequenceByPosition_ChooseLastPositonThreeSequences_ReturnsSequenceOnLastPosition()
        {
            List<Tuple<string, string>> seqs = new List<Tuple<string, string>>();
            seqs.Add(new Tuple<string, string>("erk|po3", "TATA-"));
            seqs.Add(new Tuple<string, string>("dfg|fd4", "GC-AA"));
            seqs.Add(new Tuple<string, string>("pop|mp3", "CCTAA"));

            MSA msa = new MSA(seqs);
            string s = msa.GetSequence(2);
            Assert.AreEqual("CCTAA", s);
        }

        [TestMethod]
        public void GetSequenceByPosition_OutOfBound_ReturnsNull()
        {
            List<Tuple<string, string>> seqs = new List<Tuple<string, string>>();
            seqs.Add(new Tuple<string, string>("abc", "WSARTTTAACLRA"));
            seqs.Add(new Tuple<string, string>("bbc", "W----------RA"));

            MSA msa = new MSA(seqs);
            string s = msa.GetSequence(3);
            Assert.AreEqual(null, s);
        }

        [TestMethod]
        public void GetSequenceByID_EmptyList_ReturnsNull()
        {
            List<Tuple<string, string>> seqs = new List<Tuple<string, string>>();

            MSA msa = new MSA(seqs);
            string s = msa.GetSequence("ICE|SCREAM");
            Assert.AreEqual(null, s);
        }

        [TestMethod]
        public void GetSequenceByID_ChooseIXI_234hreeSequences_ReturnsSequenceForThatName()
        {
            List<Tuple<string, string>> seqs = new List<Tuple<string, string>>();
            seqs.Add(new Tuple<string, string>("IXI_234", "SRPNRFAPTLMSSCITSTTGPPAWAGDRSHE"));
            seqs.Add(new Tuple<string, string>("IXI_235", "SRPNRFAPTL--------TGPPAWAGDRSHE"));
            seqs.Add(new Tuple<string, string>("IXI_236", "SRPPRFAPPLMSSCITSTTG-------RSHE"));

            MSA msa = new MSA(seqs);
            string s = msa.GetSequence("IXI_234");
            Assert.AreEqual("SRPNRFAPTLMSSCITSTTGPPAWAGDRSHE", s);
        }

        [TestMethod]
        public void GetSequenceByID_OutOfBound_ReturnsNull()
        {
            List<Tuple<string, string>> seqs = new List<Tuple<string, string>>();
            seqs.Add(new Tuple<string, string>("xyz", "TSPASIR"));
            seqs.Add(new Tuple<string, string>("bcd", "TSP--IR"));

            MSA msa = new MSA(seqs);
            string s = msa.GetSequence("abc");
            Assert.AreEqual(null, s);
        }
    }
}
