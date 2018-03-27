using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioAlgo;
using System.IO;
using System.Collections.Generic;

namespace BioAlgoTests
{
    [TestClass]
    public class ParserTests
    {
        string folder = @"../../input/";

        [TestMethod]
        public void FastaParser_ParseOneSequence_ReturnDictionaryWithOneKey()
        {
            String path = $@"{folder}one-entry.fasta";
            StreamReader file = new StreamReader(path);
            Dictionary<string, string> seqs = FastaParser.Parse(file);
            Assert.AreEqual(1, seqs.Keys.Count);
        }

        [TestMethod]
        public void FastaParser_ParseTenSequencesWithoutEmptyLines_ReturnDictionaryWithTenKeys()
        {
            String path = $@"{folder}ten-entries-without-empty-lines.fasta";
            StreamReader file = new StreamReader(path);
            Dictionary<string, string> seqs = FastaParser.Parse(file);
            Assert.AreEqual(11, seqs.Keys.Count);
        }

        [TestMethod]
        public void FastaParser_ParseThreeSequencesWithEmptyLines_ReturnDictionaryWithThreeKeys()
        {
            String path = $@"{folder}three-entries-with-empty-lines.fasta";
            StreamReader file = new StreamReader(path);
            Dictionary<string, string> seqs = FastaParser.Parse(file);
            Assert.AreEqual(3, seqs.Keys.Count);
        }

        [TestMethod]
        public void FastaParser_ParseEmptyFile_ReturnNull()
        {
            String path = $@"{folder}empty-file.fasta";
            StreamReader file = new StreamReader(path);
            Dictionary<string, string> seqs = FastaParser.Parse(file);
            Assert.AreEqual(null, seqs);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException), "Forbidden characters were added to molecules")]
        public void FastaParser_ParseFileWith_ThrowsException()
        {
            String path = $@"{folder}forbidden-characters.fasta";
            StreamReader file = new StreamReader(path);
            Dictionary<string, string> seqs = FastaParser.Parse(file);
        }

        [TestMethod]
        public void MSAParseClustal_ParseEmptyFile_ReturnsNull()
        {
            String path = $@"{folder}empty-file.aln";
            StreamReader file = new StreamReader(path);
            List<Tuple<string, string>> seqs = MSA.Parse(file);
            Assert.AreEqual(null, seqs);
        }

        [TestMethod]
        public void MSAParseClustal_ParseTwoSequencesWithCumulativeCountOfResiduesAndConservationInfo_ReturnsListOfTuplesWithTwoEntries()
        {
            String path = $@"{folder}two-sequences-with-cumulative-count-of-residues-and-conservation-info.aln";
            StreamReader file = new StreamReader(path);
            List<Tuple<string, string>> seqs = MSA.Parse(file);
            Assert.AreEqual(2, seqs.Count);
        }

        [TestMethod]
        public void MSAParseClustal_ParseThreeSequencesWithoutCumulativeCountOfResiduesAndConservationInfoAndHeader_ReturnsListOfTuplesWithThreeEntries()
        {
            String path = $@"{folder}three-sequences-without-cumulative-count-of-residues-and-conservation-info-and-header.aln";
            StreamReader file = new StreamReader(path);
            List<Tuple<string, string>> seqs = MSA.Parse(file);
            Assert.AreEqual(3, seqs.Count);
        }

        [TestMethod]
        public void MSAParseClustal_ParseFourSequencesWithoutCumulativeCountOfResiduesAndConservationInfo_ReturnsListOfTuplesWithFourEntries()
        {
            String path = $@"{folder}four-sequences-without-cumulative-count-of-residues-and-conservation-info.aln";
            StreamReader file = new StreamReader(path);
            List<Tuple<string, string>> seqs = MSA.Parse(file);
            Assert.AreEqual(4, seqs.Count);
        }

        [TestMethod]
        public void MSAParseClustal_InvalidInput_ReturnsNull()
        {
            String path = $@"{folder}invalid-input.aln";
            StreamReader file = new StreamReader(path);
            List<Tuple<string, string>> seqs = MSA.Parse(file);
            Assert.AreEqual(null, seqs);
        }
    }
}
