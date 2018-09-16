using BioAlgo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace BioAlgoTests
{
    [TestClass]
    public class ParserTests
    {
        private string folder = @"../../input/";

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
        [ExpectedException(typeof(InvalidDataException))]
        public void FastaParser_ParseEmptyFile_ReturnNull()
        {
            String path = $@"{folder}empty-file.fasta";
            StreamReader file = new StreamReader(path);
            Dictionary<string, string> seqs = FastaParser.Parse(file);
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

        [TestMethod]
        public void PDBParse_SimpleInput()
        {
            String path = $@"{folder}simple.pdb";
            StreamReader file = new StreamReader(path);
            List<PDB.Model> models = PDB.Parse(file);
            foreach (PDB.Model model in models)
            {
                Console.WriteLine(model);
            }
            Assert.AreEqual(models.Count, 2);
            Assert.AreEqual(models[0].NumOfChains(), 1);
            Assert.AreEqual(models[0].NumOfResidues(0), 2);
            Assert.AreEqual(models[0].NumOfAtoms(0, "1"), 2);
            Assert.AreEqual(models[0].NumOfAtoms(0, "18"), 2);
            Assert.AreEqual(models[1].NumOfChains(), 1);
            Assert.AreEqual(models[1].NumOfResidues(0), 2);
            Assert.AreEqual(models[1].NumOfAtoms(0, "1"), 2);
            Assert.AreEqual(models[1].NumOfAtoms(0, "18"), 2);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void PDBParse_SimpleInput_FailsNumOfAtomsForMissingChain()
        {
            String path = $@"{folder}simple.pdb";
            StreamReader file = new StreamReader(path);
            List<PDB.Model> models = PDB.Parse(file);
            Assert.AreEqual(models[0].NumOfChains(), 1);
            Assert.AreEqual(models[0].NumOfResidues(0), 2);
            Assert.AreEqual(models[0].NumOfResidues(1), 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PDBParse_SimpleInput_FailsNumOfAtomsForMissingResidue()
        {
            String path = $@"{folder}simple.pdb";
            StreamReader file = new StreamReader(path);
            List<PDB.Model> models = PDB.Parse(file);
            Assert.AreEqual(models[0].NumOfChains(), 1);
            Assert.AreEqual(models[0].NumOfResidues(0), 2);
            Assert.AreEqual(models[0].NumOfAtoms(0, "1"), 2);
            Assert.AreEqual(models[0].NumOfAtoms(0, "18"), 2);
            Assert.AreEqual(models[0].NumOfAtoms(0, "28"), 2);
        }

        [TestMethod]
        public void PDBParse_ChainsInput()
        {
            String path = $@"{folder}chains.pdb";
            StreamReader file = new StreamReader(path);
            List<PDB.Model> models = PDB.Parse(file);
            foreach (PDB.Model model in models)
            {
                Console.WriteLine(model);
            }
            Assert.AreEqual(models.Count, 1);
            Assert.AreEqual(models[0].NumOfChains(), 2);
            Assert.AreEqual(models[0].NumOfResidues(0), 2);
            Assert.AreEqual(models[0].NumOfAtoms(0, "1"), 2);
            Assert.AreEqual(models[0].NumOfAtoms(0, "18"), 2);
            Assert.AreEqual(models[0].NumOfResidues(1), 1);
            Assert.AreEqual(models[0].NumOfAtoms(1, "21"), 4);
            Assert.AreEqual(models[0].NumOfHeteroAtoms(), 4);
        }

        [TestMethod]
        public void PDBParse_ComputingChainsWidth()
        {
            String path = $@"{folder}chains.pdb";
            StreamReader file = new StreamReader(path);
            List<PDB.Model> models = PDB.Parse(file);
            Assert.AreEqual(29.185455076116, models[0].chains[0].Width(), 0.00001);
            Assert.AreEqual(27.1619058977827, models[0].chains[1].Width(), 0.00001);
        }

        [TestMethod]
        public void PDBParse_ComputingResidueWidth()
        {
            String path = $@"{folder}chains.pdb";
            StreamReader file = new StreamReader(path);
            List<PDB.Model> models = PDB.Parse(file);
            Assert.AreEqual(1.46001472595313, models[0].chains[0].GetResidue("1").Width(), 0.00001);
            Assert.AreEqual(1.74426546144788, models[0].chains[0].GetResidue("18").Width(), 0.00001);
            Assert.AreEqual(27.1619058977827, models[0].chains[1].GetResidue("21").Width(), 0.00001);
        }

        [TestMethod]
        public void PDBParse_ComputesAtomsNearHeteroAtom()
        {
            String path = $@"{folder}chains.pdb";
            StreamReader file = new StreamReader(path);
            List<PDB.Model> models = PDB.Parse(file);
            Assert.AreEqual(1, models[0].chains[0].AtomsNearHeteroAtom(models[0].hetero_atoms[0], 1.25).Count);
            Assert.AreEqual(2, models[0].chains[0].AtomsNearHeteroAtom(models[0].hetero_atoms[0], 5.25).Count);
            Assert.AreEqual(1, models[0].chains[1].AtomsNearHeteroAtom(models[0].hetero_atoms[0], 1.25).Count);
            Assert.AreEqual(2, models[0].chains[1].AtomsNearHeteroAtom(models[0].hetero_atoms[0], 5.25).Count);
            Assert.AreEqual(4, models[0].chains[1].AtomsNearHeteroAtom(models[0].hetero_atoms[0], 30).Count);
            Assert.AreEqual(8, models[0].AtomsNearHeteroAtom(models[0].hetero_atoms[0], 30).Count);
        }

        [TestMethod]
        public void PDBParse_ComputesResiduesNearHeteroAtom()
        {
            String path = $@"{folder}chains.pdb";
            StreamReader file = new StreamReader(path);
            List<PDB.Model> models = PDB.Parse(file);
            Assert.AreEqual(1, models[0].chains[0].ResiduesNearHeteroAtom(models[0].hetero_atoms[0], 1.25).Count);
            Assert.AreEqual(2, models[0].chains[0].ResiduesNearHeteroAtom(models[0].hetero_atoms[0], 30).Count);
            Assert.AreEqual(3, models[0].ResiduesNearHeteroAtom(models[0].hetero_atoms[0], 30).Count);
        }
    }
}