using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DnaAnaliser
{
    internal class GenomAnalise
    {
        #region GeenCombos

        public static List<GenomType[]> GeenCombos = new List<GenomType[]>(32)  {   new GenomType[2] {GenomType.A, GenomType.A },
                                                                                    new GenomType[2] {GenomType.A, GenomType.C },
                                                                                    new GenomType[2] {GenomType.A, GenomType.G },
                                                                                    new GenomType[2] {GenomType.A, GenomType.T },
                                                                                    new GenomType[2] {GenomType.A, GenomType.NaG },

                                                                                    new GenomType[2] {GenomType.C, GenomType.A },
                                                                                    new GenomType[2] {GenomType.C, GenomType.C },
                                                                                    new GenomType[2] {GenomType.C, GenomType.G },
                                                                                    new GenomType[2] {GenomType.C, GenomType.T },
                                                                                    new GenomType[2] {GenomType.C, GenomType.NaG },

                                                                                    new GenomType[2] {GenomType.G, GenomType.A },
                                                                                    new GenomType[2] {GenomType.G, GenomType.C },
                                                                                    new GenomType[2] {GenomType.G, GenomType.G },
                                                                                    new GenomType[2] {GenomType.G, GenomType.T },
                                                                                    new GenomType[2] {GenomType.G, GenomType.NaG },

                                                                                    new GenomType[2] {GenomType.T, GenomType.A },
                                                                                    new GenomType[2] {GenomType.T, GenomType.C },
                                                                                    new GenomType[2] {GenomType.T, GenomType.G },
                                                                                    new GenomType[2] {GenomType.T, GenomType.T },
                                                                                    new GenomType[2] {GenomType.T, GenomType.NaG },

                                                                                    new GenomType[2] {GenomType.NaG, GenomType.A },
                                                                                    new GenomType[2] {GenomType.NaG, GenomType.C },
                                                                                    new GenomType[2] {GenomType.NaG, GenomType.G },
                                                                                    new GenomType[2] {GenomType.NaG, GenomType.T },
                                                                                    new GenomType[2] {GenomType.NaG, GenomType.NaG },
        };

        #endregion GeenCombos

        public string Name { get; private set; } = "Unnamed";

        public int Count => _geens.Count;

        private List<Gen> _geens = new List<Gen>(1000000);

        public GenomAnalise(string PathTo23AndMeFile)
        {
            try
            {
                Name = PathTo23AndMeFile.Split('\\').Last();
                var Geens = File.ReadAllLines(PathTo23AndMeFile);
                _geens = (from a in Geens
                          select Gen.Parse(a) into tmp
                          where tmp != null
                          select tmp).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public GenomAnalise(List<Gen> Geens)
        {
            _geens.AddRange(Geens);
        }

        public List<KeyValuePair<GenomType[], int>> Distribution()
        {
            return (from Combo in GeenCombos.AsParallel()
                    select new KeyValuePair<GenomType[], int>(Combo, _geens.Count(i => i.GeenoTypes.Count() == Combo.Count() && (Combo.Count() == 0 ? true : (
                                Combo.Count() == 1 ? (Combo[0] == i.GeenoTypes[0]) : (
                                Combo.Count() == 2 ? (Combo[0] == i.GeenoTypes[0] && Combo[1] == i.GeenoTypes[1]) : false)))))).OrderBy(i => i.Value).ToList();
        }

        public string DistributionToString(int indent, bool ShowZeros)
        {
            string ret = string.Empty;
            foreach (var Dis in Distribution())
            {
                if (!ShowZeros)
                    ret += Dis.Value == 0 ? string.Empty : $"{new string(' ', indent)}GeenCombo: {GeenComboToString(Dis.Key)} Count: {Dis.Value}, {Math.Round((double)100 * Dis.Value / Count, 3)}%{Environment.NewLine}";
            }
            return ret;
        }

        public string DistributionToString(int indent)
        {
            return DistributionToString(indent, false);
        }

        public string DistributionToString()
        {
            return DistributionToString(0);
        }

        public List<KeyValuePair<Chromosone, GenomAnalise>> SplitToChromosones()
        {
            return (from chrom in this._geens
                    group chrom by chrom.Chromosone into newG
                    select newG into arr
                    select new KeyValuePair<Chromosone, GenomAnalise>(arr.Key, new GenomAnalise(arr.ToList()))).ToList();
        }

        public static string GeenComboToString(GenomType[] GenoComb)
        {
            return new string((from geen in GenoComb
                               from c in geen.ToString()
                               select c).ToArray());
        }
    }
}