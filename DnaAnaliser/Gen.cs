using System.Linq;

namespace DnaAnaliser
{
    internal class Gen
    {
        public string Rsid { get; set; }
        public Chromosone Chromosone { get; set; }
        public int Position { get; set; }
        public GenomType[] GeenoTypes { get; set; }

        public Gen(string Rsid, Chromosone Chromo, int Position, GenomType[] GeenoTypes)
        {
            this.Rsid = Rsid;
            this.Chromosone = Chromo;
            this.Position = Position;
            this.GeenoTypes = GeenoTypes;
        }

        static public Gen Parse(string FormatedString)
        {
            var b = FormatedString.Split('	');
            if (b[0] == "rsid" || b.Length != 4)
                return null;
            Chromosone TryP;
            TryP = Chromosone.TryParse(b[1], out TryP) ? TryP : Chromosone.NaC;
            var Gtype = (from Geenon in b[3]
                         select Geenon == 'A' ? GenomType.A : (
                                Geenon == 'C' ? GenomType.C : (
                                Geenon == 'G' ? GenomType.G :
                                Geenon == 'T' ? GenomType.T : GenomType.NaG))).ToList();
            if (Gtype.Count == 1)
                Gtype.Add(GenomType.NaG);
            int numb;
            if (!int.TryParse(b[2], out numb))
                numb = 0;

            return new Gen(b[0], TryP, numb, Gtype.ToArray());
        }
    }
}