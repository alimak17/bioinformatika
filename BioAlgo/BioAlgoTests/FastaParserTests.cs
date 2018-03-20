using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BioAlgo;
using System.IO;
using System.Collections.Generic;

namespace BioAlgoTests
{
    [TestClass]
    public class FastaParserTests
    {
        string folder = @"../../input/";

        [TestMethod]
        public void FastaParser_ParseOneSequence_ReturnDictionaryWithOneKeyAndValue()
        {
            String path = $@"{folder}one-entry.fasta";
            StreamReader file = new StreamReader(path);
            Dictionary<string, string> seqs = FastaParser.Parse(file);
            Assert.AreEqual(1, seqs.Keys.Count);
        }

        [TestMethod]
        public void FastaParser_ParseTenSequencesWithoutEmptyLines_ReturnDictionaryWithTenKeysAndEntries()
        {
            String path = $@"{folder}ten-entries-without-empty-lines.fasta";
            StreamReader file = new StreamReader(path);
            Dictionary<string, string> seqs = FastaParser.Parse(file);
            Assert.AreEqual(11, seqs.Keys.Count);
        }

        [TestMethod]
        public void FastaParser_ParseThreeSequencesWithEmptyLines_ReturnDictionaryWithThreeKeysAndEntries()
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
    }
}
