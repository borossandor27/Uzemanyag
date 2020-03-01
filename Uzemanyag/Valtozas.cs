using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uzemanyag
{
    class Valtozas
    {
        DateTime datum;
        double benzin;
        double gazolaj;

        public DateTime Datum { get => datum; set => datum = value; }
        public double Benzin { get => benzin; set => benzin = value; }
        public double Gazolaj { get => gazolaj; set => gazolaj = value; }

        public Valtozas(DateTime datum, double benzin, double gazolaj)
        {
            Datum = datum;
            Benzin = benzin;
            Gazolaj = gazolaj;
        }
    }
}
