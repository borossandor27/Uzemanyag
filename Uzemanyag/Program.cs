using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Uzemanyag
{
    class Program
    {
        static List<Valtozas> valtozasok = new List<Valtozas>();

        static void Main(string[] args)
        {
            Beolvas(@"..\..\uzemanyag.txt");
            
            Console.WriteLine($"\n3. feladat: Változások száma: {valtozasok.Count}");

            double min = valtozasok.Min(a => Math.Abs(a.Benzin - a.Gazolaj));
            Console.WriteLine($"\n4. feladat: A legkisebb különbség: {min}");

            Console.WriteLine($"\n5. feladat: A legkisebb különbség előfordulása: {valtozasok.Count(a => Math.Abs(a.Gazolaj - a.Benzin) == min)}");

            Console.WriteLine($"\n6. feladat: {Feladat_06()}");

            Feladat_07();

            int evszam = Feladat_08();

            Console.WriteLine($"10. feladat: {evszam} évben a leghosszabb időszak {Feladat_10(evszam)} nap volt.");


            Console.WriteLine();
            Console.WriteLine("\nProgram vége!");
            Console.ReadKey();

        }
        /// <summary>
        /// Olvassa be az uzemanyag.txt állományban lévő adatokat és tárolja el egy 
        /// olyan adatszerkezetben, ami a további feladatok megoldására alkalmas! 
        /// </summary>
        /// <param name="forras">uzemanyag.txt elérési úttal</param>
        static void Beolvas(string forras)
        {
            Console.WriteLine("2. feladat: forrásadatok beolvasása...");
            try
            {
                using (StreamReader sr = new StreamReader(forras))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] sor = sr.ReadLine().Split(';');
                        valtozasok.Add(new Valtozas(DateTime.Parse(sor[0]), double.Parse(sor[1]), double.Parse(sor[2])));
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        /// <summary>
        /// 6. Döntse el, hogy a vizsgált időszakban volt-e szökőnapon árváltozás! 
        /// A megoldását a következők alapján készítse el:
        /// - Szökőnap február 24-én van, ha az év szökőév.
        /// - A megadott időszakban az év szökőév, ha az néggyel osztható maradék nélkül.
        /// Az eredmény jelenjen meg a képernyőn is a minta szerint!
        /// </summary>
        /// <returns>A kiírandó szöveg</returns>
        static string Feladat_06()
        {
            if (valtozasok.Count(a => a.Datum.Year % 4 == 0 && a.Datum.Month == 2 && a.Datum.Day == 24) > 0)
            {
                return "Volt változás szökőnapon!";
            }
            else
            {
                return "Nem volt változás szökőnapon!";
            }
        }
        /// <summary>
        /// 7. Készítsen szöveges állományt euro.txt néven, melynek szerkezete 
        ///    megegyezik az uzemanyag.txt állománnyal.
        ///    A különbség annyi legyen, hogy az üzemanyagárakat euró valutanembe 
        ///    átszámolva, két tizedesjegy pontossággal tartalmazza! Az átváltási
        ///    árfolyamot valós típusú konstansként rögzítse megoldásában! 
        ///    Egy euró 307,7Ft legyen!
        ///    Az állomány szerkezetére a feladat végén talál mintát!
        /// </summary>
        static void Feladat_07()
        {
            Console.WriteLine("\n7. feladat: Szövegfájl létrehozása ...");
            double arf = 307.7; //-- az euro árfolyama

            using (StreamWriter sw = new StreamWriter("euro.txt"))
            {
                foreach (Valtozas item in valtozasok)
                {
                    sw.WriteLine(string.Join(";", item.Datum.ToString("yyyy.M.dd"), (item.Benzin / arf).ToString("0.00"), (item.Gazolaj / arf).ToString("0.00")));
                }
            }
        }
        /// <summary>
        /// 8. Kérjen be egy évszámot a felhasználótól a minta szerint! 
        ///    Az adatbevitelt mindaddig ismételje, amíg nem teljesül a 
        ///    következő feltétel: 
        ///     2011 ≤ évszám ≤ 2016!
        /// </summary>
        /// <returns>Az évszám</returns>
        static int Feladat_08()
        {
            int evszam;
            do
            {
                Console.Write("8. feladat: Kérem adja meg az évszámot [2011..2016]: ");
            } while (!(int.TryParse(Console.ReadLine(), out evszam) && evszam>=2011 && evszam<=2016));
            return evszam;
        }
        /// <summary>
        /// 9. Készítsen függvényt vagy metódust, amivel meghatározza két, 
        /// egymást követő árváltozás között eltelt napok számát! 
        /// </summary>
        /// <param name="aktuálisVáltozás">Aktuális változás napja</param>
        /// <param name="előzőVáltozás">Előző változás napja</param>
        /// <returns></returns>
        static int Feladat_09(DateTime aktualisValtozas, DateTime elozoValtozas)
        {
            int eltelt;
            int[] napokSzama;
            if (aktualisValtozas.Year % 4 == 0)
            {
                //-- szökőév ------------------------------------------
                napokSzama = new int[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            }
            else
            {
                //-- nem szökőév ---------------------------------------
                napokSzama = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            }
            if (aktualisValtozas.Month == elozoValtozas.Month)
            {
                //-- ugyanabban a hónapban vannak -----------------------
                return aktualisValtozas.Day - elozoValtozas.Day;
            }
            else
            {
                //-- elozo hónapban vannak ------------------------------
                return napokSzama[elozoValtozas.Month - 1] - elozoValtozas.Day + aktualisValtozas.Day;
            }
        }

        static double Feladat_10(int ev)
        {
            double lh = 0;
            for (int i = 0; i < valtozasok.Count - 1; i++)
            {
                if (valtozasok[i].Datum.Year == ev && (valtozasok[i + 1].Datum - valtozasok[i].Datum).TotalDays > lh)
                {
                    lh = (valtozasok[i + 1].Datum - valtozasok[i].Datum).TotalDays;
                }
            }
            return lh;
        }
    }
}
