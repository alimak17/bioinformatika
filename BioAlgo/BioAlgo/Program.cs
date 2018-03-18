using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace BioAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            string a = "red";
            WriteLine(SequenceAlignment.CountEditDistance(a, a));
        }
    }
}
