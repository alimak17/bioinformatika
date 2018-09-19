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
        private static void AnalyzeStructureOfPDB(string path)
        {
            WriteLine($"Analyzing {path}.");
            StreamReader file = new StreamReader(path);
            List<Model> models = Parse(file);
            foreach (Model model in models)
            {
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

        private static void Main(string[] args)
        {
            AnalyzeStructureOfPDB(@"../../input/Hemoglobin_(1b0b).pdb");
            AnalyzeStructureOfPDB(@"../../input/A2a_receptor.pdb");
            
            ReadKey();
        }
    }
}