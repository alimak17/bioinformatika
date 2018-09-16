using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace BioAlgo
{
    // Wrapper of PDB files.
    // http://www.wwpdb.org/documentation/file-format
    public class PDB
    {
        static double AtomDistance(Atom a, Atom b)
        {
            double x = a.x - b.x;
            double y = a.y - b.y;
            double z = a.z - b.z;
            return Math.Sqrt(x * x + y * y + z * z);
        }

        static double computeWidth(List<Atom> atoms)
        {
            double width = 0.0;
            for (int i = 0; i < atoms.Count; i++)
            {
                for (int j = i+1; j < atoms.Count; j++)
                {
                    double distance = AtomDistance(atoms[i], atoms[j]);
                    if (distance > width)
                    {
                        width = distance;
                    }
                }
            }
            return width;
        }

        public class Atom
        {
            public int id;
            public string name;
            public char altLoc; // Alternate location indicator
            public string resName; // Residue name
            public char chainId;
            public string resSeq; // Residue sequence number
            public char iCode; // Code for insertion of residues.
            public string type;
            public double x;
            public double y;
            public double z;
            public double occupancy;
            public double temperature;
            public string element;
            public string charge;

            public Atom(string data)
            {
                CultureInfo en = new CultureInfo("en");
                try
                {
                    id = int.Parse(data.Substring(6, 5));
                    name = data.Substring(12, 4).Trim();
                    altLoc = data[16];
                    resName = data.Substring(17, 3).Trim();
                    chainId = data[21];
                    resSeq = data.Substring(22, 4).Trim();
                    iCode = data[26];
                    x = double.Parse(data.Substring(30, 8), en);
                    y = double.Parse(data.Substring(38, 8), en);
                    z = double.Parse(data.Substring(46, 8), en);
                    double.TryParse(data.Substring(54, 6), NumberStyles.Any, en, out occupancy);
                    double.TryParse(data.Substring(60, 6), NumberStyles.Any, en, out temperature);
                    element = data.Substring(76, 2).Trim();
                    charge = data.Substring(78, 2).Trim();
                } catch (ArgumentOutOfRangeException) {
                }
            }

            public override string ToString()
            {
                return $"Atom: id = {id}, name = {name}, altLoc = {altLoc}, resName = {resName}, " +
                    $"chainId = {chainId}, resSeq = {resSeq}, iCode = {iCode}, pos = ({x}, {y}, {z}), " +
                    $"occupancy = {occupancy}, temperature = {temperature}, element = {element}, charge = {charge}\n";
            }
        }

        public class Residue
        {
            public string number;
            public List<Atom> atoms = new List<Atom>();

            public Residue(string number)
            {
                this.number = number;
            }

            public override string ToString()
            {
                string ret = $"Residue {number} start\n";
                foreach (Atom atom in atoms)
                {
                    ret += atom.ToString();
                }
                return ret + "Residue end\n";
            }
            
            // Compute width of the residue.
            public double Width()
            {
                return computeWidth(atoms);
            }

            public bool HasAtomsNearHeteroAtom(Atom hetero_atom, double distance)
            {
                foreach (Atom atom in atoms)
                {
                    if (AtomDistance(hetero_atom, atom) <= distance)
                        return true;
                }
                return false;
            }
        }

        public class Chain
        {
            public List<Residue> residues = new List<Residue>();

            public override string ToString()
            {
                string ret = "Chain start\n";
                foreach (Residue residue in residues)
                {
                    ret += residue.ToString();
                }
                return ret + "Chain end\n";
            }

            public Residue GetResidue(string number)
            {
                int index;
                if (ContainsResidue(number, out index))
                {
                    return residues[index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"No residue '{number}'.");
                }

            }

            public bool ContainsResidue(string number, out int index)
            {
                for (int i = 0; i < residues.Count; i++)
                {
                    if (number == residues[i].number)
                    {
                        index = i;
                        return true;
                    }
                }
                index = -1;
                return false;
            }

            public void AddAtom(Atom atom)
            {
                int index;
                if (!ContainsResidue(atom.resSeq, out index))
                {
                    residues.Add(new Residue(atom.resSeq));
                    index = residues.Count - 1;
                }
                residues[index].atoms.Add(atom);
            }

            // Compute width of the chain.
            public double Width()
            {
                List<Atom> all_atoms = new List<Atom>();
                foreach (Residue residue in residues)
                {
                    foreach (Atom atom in residue.atoms)
                    {
                        all_atoms.Add(atom);
                    }
                }
                return computeWidth(all_atoms);
            }

            public List<Atom> AtomsNearHeteroAtom(Atom hetero_atom, double distance)
            {
                List<Atom> atoms = new List<Atom>();
                foreach (Residue residue in residues)
                {
                    foreach (Atom atom in residue.atoms)
                    {
                        if (AtomDistance(hetero_atom, atom) <= distance)
                            atoms.Add(atom);
                    }
                }
                return atoms;
            }

            public List<Residue> ResiduesNearHeteroAtom(Atom hetero_atom, double distance)
            {
                List<Residue> residues_near = new List<Residue>();
                foreach (Residue residue in residues)
                {
                    if (residue.HasAtomsNearHeteroAtom(hetero_atom, distance)) {
                        residues_near.Add(residue);
                    }
                }
                return residues_near;
            }
        }

        public class Model
        {
            public string id;
            public List<Chain> chains = new List<Chain>();
            public List<Atom> hetero_atoms = new List<Atom>();

            public override string ToString()
            {
                string ret = $"Model {id} start\n";
                foreach (Chain chain in chains)
                {
                    ret += chain.ToString();
                }
                foreach (Atom atom in hetero_atoms)
                {
                    ret += "Hetero" + atom.ToString();
                }
                return ret + "Model end\n";
            }

            public void AddAtom(Atom atom, int chain_num)
            {
                chains[chain_num].AddAtom(atom);
            }

            public int NumOfChains() => chains.Count;
            public int NumOfHeteroAtoms() => hetero_atoms.Count;
            public int NumOfResidues(int chain_num)
            {
                if (chain_num >= chains.Count)
                {
                    throw new IndexOutOfRangeException($"No chain {chain_num}, model has only " + NumOfChains() + "chains.");
                }
                return chains[chain_num].residues.Count;
            }

            public int NumOfAtoms(int chain_num, string residue_number)
            {
                if (chain_num >= chains.Count)
                {
                    throw new IndexOutOfRangeException($"No chain {chain_num}, model has only " + NumOfChains() + "chains.");
                }
                return chains[chain_num].GetResidue(residue_number).atoms.Count;
            }

            public List<Atom> AtomsNearHeteroAtom(Atom hetero_atom, double distance)
            {
                List<Atom> atoms = new List<Atom>();
                foreach (Chain chain in chains)
                {
                    atoms.AddRange(chain.AtomsNearHeteroAtom(hetero_atom, distance));
                }
                return atoms;
            }

            public List<Residue> ResiduesNearHeteroAtom(Atom hetero_atom, double distance)
            {
                List<Residue> residues= new List<Residue>();
                foreach (Chain chain in chains)
                {
                    residues.AddRange(chain.ResiduesNearHeteroAtom(hetero_atom, distance));
                }
                return residues;
            }
        }

        List<Model> models;

        public PDB(StreamReader pdbFile)
            => models = Parse(pdbFile);

        public static List<Model> Parse(StreamReader pdbFile)
        {
            List<Model> models = new List<Model>();
            String line;
            Model current_model = new Model();
            int opened_chain = -1;
            while ((line = pdbFile.ReadLine()) != null)
            {
                switch (line.Substring(0,6))
                {
                    case "MODEL ":
                        current_model.id = line.Substring(10,4);
                        break;
                    case "ENDMDL":
                        models.Add(current_model);
                        current_model = new Model();
                        opened_chain = -1;
                        break;                        
                    case "ATOM  ":
                        if (opened_chain == -1) {
                            current_model.chains.Add(new Chain());
                            opened_chain = current_model.chains.Count - 1;
                        }
                        current_model.AddAtom(new Atom(line), opened_chain);
                        break;
                    case "HETATM":
                        current_model.hetero_atoms.Add(new Atom(line));
                        break;
                    case "TER   ":
                        opened_chain = -1;
                        break;
                }
            }
            if (current_model.chains.Count > 0 || current_model.hetero_atoms.Count > 0)
            {
                models.Add(current_model);
            }

            pdbFile.Close();
            if (models.Count == 0)
                return null;
            else
                return models;
        }
    }
}
