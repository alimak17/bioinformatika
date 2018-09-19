# Bioinformatics Toolbox

Solution of assignments from [Bioinformatics toolbax](http://bioinformatika.mff.cuni.cz/repository/#/stories/detail?id=bioinformatics_toolbox).

##Processing FASTA files##

Implemented in `FastaParser.cs` and used in `ParserTests.cs`.

##Measuring sequance similarity using Hamming distance##

Implemented in `PairwiseSequenceAlignment.cs` (CountHammingDistance function) and used in `BioAlgoTests.cs`.

##Sequence alignment using edit distance##

Implemented in `PairwiseSequenceAlignment.cs` (CountEditDistance and FindAllAlignmentsUsingEditDistance functions) and used in `BioAlgoTests.cs`.

##Processing PDB files$$

Implemented in `PDB.cs` and used in `ParserTests.cs`.

##Processing multiple sequence alignment##

Implemented in `MultipleSequnceAlignment.cs` and tested in `ParserTests.cs` and `BioAlgoTests.cs`.

##Conservation determination for multiple aligned sequences##

Implemented in `MultipleSequnceAlignment.cs` (GetScores function) and tested in `ParserTests.cs`.

##Computing structure-related properties##

Implemented in `PDB.cs` (ComputeSurfaceAndBuriedAminoAcids function), used in `Program.cs`. Data for histogram are written in human-readable form. Output analysis can be found in `input/source_analysis.txt`.

