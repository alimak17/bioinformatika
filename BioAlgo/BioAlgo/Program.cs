using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static BioAlgo.PDB;
using static System.Console;

namespace BioAlgo
{
    internal class Program
    {
        private static void ComputePairwiseSequenceAlignment(string path, bool all_alignments)
        {
            WriteLine($"Parsing FASTA file {path}.");
            StreamReader file = new StreamReader(path);
            Dictionary<string, string> seqs = FastaParser.Parse(file);
            WriteLine("Parsed sequences:");
            foreach (KeyValuePair<string, string> seq in seqs)
            {
                WriteLine(seq.Key + ": " + seq.Value);
            }

            WriteLine("Pairwise sequence alignments:");
            foreach (KeyValuePair<string, string> pair in seqs)
            {
                foreach (KeyValuePair<string, string> pair2 in seqs)
                {
                    string name = pair.Key;
                    string name2 = pair2.Key;
                    string seq = pair.Value;
                    string seq2 = pair2.Value;
                    if (name.CompareTo(name2) >= 0)
                        continue;
                    WriteLine($"{name} vs {name2}: ");
                    WriteLine("Edit distance: " + PSA.CountEditDistance(seq, seq2));
                    if (seq.Length == seq2.Length)
                    {
                        WriteLine("Hamming distance: " + PSA.CountHammingDistance(seq, seq2));
                    }
                    if (all_alignments)
                    {
                        WriteLine("All optimal alignments: ");
                        List<Tuple<string, string>> alignments = PSA.FindAllAlignmentsUsingEditDistance(seq, seq2);
                        foreach (Tuple<string, string> alignment in alignments)
                        {
                            WriteLine(alignment.Item1);
                            WriteLine(alignment.Item2);
                            WriteLine();
                        }
                    }
                }
            }  
        }

        private static void ComputeConservationScores(string path, int top_k)
        {
            WriteLine($"Parsing clustal file {path}.");
            Matrix m = new Matrix("blosum62.bla");
            StreamReader file = new StreamReader(path);
            MSA msa = new MSA(file);
            WriteLine($"Parsed {msa.sequences.Count} sequences:");
            foreach (Tuple<string, string> seq in msa.sequences)
            {
                WriteLine($"{seq.Item1}: {seq.Item2}");
            }

            WriteLine("Conservation scores:");
            int[] scores = msa.GetScores(m, -1);
            int total_score = scores.Sum();
            foreach (int score in scores)
            {
                Write($"{score}, ");
            }
            WriteLine();
            WriteLine($"Total conservation score: {total_score}.");
            if (top_k > msa.GetLength())
                return;
            WriteLine($"Top {top_k} conservation scores:");
            List<Tuple<int, int>> top_scores = msa.TopScores(scores, top_k);
            for (int i = 0; i < top_k; i++)
            {
                WriteLine($"#{i+1} at position {top_scores[i].Item1} with score {top_scores[i].Item2}.");
            }
        }

        private static void AnalyzeStructureOfPDB(string path)
        {
            WriteLine($"Parsing {path}.");
            StreamReader file = new StreamReader(path);
            List<Model> models = Parse(file);
            foreach (Model model in models)
            {
                WriteLine($"Analyzing the model {model.id}.");
                Tuple<List<string>, List<string>> lists = model.ComputeSurfaceAndBuriedAminoAcids();
                List<string> surface = lists.Item1;
                List<string> buried = lists.Item2;
                WriteLine("\nAnalysis done:");
                foreach (Chain chain in model.chains)
                {
                    WriteLine($"Diameter of protein: {chain.Width()}.");
                }
                WriteLine($"Count of surface amino acids: {surface.Count}.");
                WriteLine($"Count of buried amino acids: {buried.Count}.");
                double ratio = (double) surface.Count / (double) buried.Count;
                WriteLine($"Ratio of surface and buried amino acids: {ratio}.");

                var surface_names = model.ResiduesToAminoAcidNames(surface).GroupBy(i => i);
                var buried_names = model.ResiduesToAminoAcidNames(buried).GroupBy(i => i);
                Write($"Surface amino acids: ");
                foreach (var name in surface_names)
                {
                    Write($"{name.Count()}x {name.Key}, ");
                }
                WriteLine();
                Write($"Buried amino acids: ");
                foreach (var name in buried_names)
                {
                    Write($"{name.Count()}x {name.Key}, ");
                }
                WriteLine();

                List<string> polar_surface = model.SelectPolarAminoAcids(surface);
                List<string> polar_buried = model.SelectPolarAminoAcids(buried);
                WriteLine($"Count of surface polar amino acids: {polar_surface.Count}.");
                WriteLine($"Count of buried polar amino acids: {polar_buried.Count}.");
                ratio = (double) polar_surface.Count / (double) polar_buried.Count;
                WriteLine($"Ratio of surface and buried polar amino acids: {ratio}.");
                WriteLine();
            }
        }

        // Possible arguments:
        //  fasta [file] ... reads a fasta file, for each pair of sequences,
        //                   computes optimal alignment and Hamming distance (if of the same length).
        //                   Use optional third parameter all to output all optimal alignments.
        //  msa [file] [k] ... reads a crustal file and computes conservation scores and shows top k scores.
        //  pdb [file] ... reads a PDB file and computes structural analysis of the protein.
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                WriteLine("Needs two arguments, the first one fasta, msa, or pdb, the second one a path to an input file.");
            }
            else if (args[0] == "msa" && args.Length < 3)
            {
                WriteLine("For msa needs two other arguments: input file and number of positions with top scores.");
            }
            else
            {
                switch (args[0])
                {
                    case "fasta":
                        if (args.Length > 2 && args[2] == "all")
                            ComputePairwiseSequenceAlignment(args[1], true);
                        else
                            ComputePairwiseSequenceAlignment(args[1], false);
                        break;
                    case "msa":
                        ComputeConservationScores(args[1], int.Parse(args[2]));
                        break;
                    case "pdb":
                        AnalyzeStructureOfPDB(args[1]);
                        break;
                    default:
                        WriteLine("The first argument has to be equal fasta, msa, or pdb.");
                        break;
                }
            }
            WriteLine("<press any key to exit>");
            ReadKey();
        }
    }
}