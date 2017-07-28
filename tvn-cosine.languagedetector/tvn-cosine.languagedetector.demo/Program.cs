using System; 

namespace tvn_cosine.languagedetector.demo
{
    class Program
    {
        static string[] inputTexts = { @"Western Cape Premier Helen Zille is due to face a motion of no confidence in the provincial legislature following her infamous tweets about colonialism."
                                     , @"Die polisie het Donderdagoggend die betogers wat strate in die Grassy Park-omgewing van Kaapstad versper het met waterkanonne, traanrook en rubberkoeëls uiteengejaag. Hulle het reeds van vroeg af oor swak dienslewering betoog."
                                     , @"Ulahlwe ngelakubo owesilisa kwa-CC eMlazi, eningizimu yeTheku, okusolwa ukuthi ubezama ukudlwengula umfana oneminyaka engu-15 ubudala njengoba eshaywe wabulawa ngamalungu omphakathi ebusuku."
                                     , @"Leia as Normas do Forum por favor!"};

        static void Main(string[] args)
        {
            DetectorFactory.loadProfile("./profiles/");
            foreach (string input in inputTexts)
            {
                Detector detector = DetectorFactory.create();
                detector.append(input);
                Console.WriteLine("Lang: {0}", detector.detect());
                foreach (var prob in detector.getProbabilities())
                {
                    Console.WriteLine("Other: {0} at P({1})", prob.lang, prob.prob);
                }
                Console.WriteLine();
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
