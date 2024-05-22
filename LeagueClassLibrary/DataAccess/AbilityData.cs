using LeagueClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassLibrary.DataAccess
{
    public static class AbilityData
    {
        private static List<Ability> Abilities {  get; set; }

        public static void LoadCSV(string padNaarCsv)
        {
            using(StreamReader sr = new StreamReader(padNaarCsv))
            {
                if(!sr.EndOfStream)
                {
                    string headers = sr.ReadLine();
                    string[] headerParts = headers.Split(';');
                    if(headerParts.Length == 3 )
                    {
                        Abilities = new List<Ability>();
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] parts = line.Split(';');
                            if (parts.Length == 3)
                            {
                                Ability ability = new Ability(int.Parse(parts[0]), parts[1], parts[2]);
                                Abilities.Add(ability);
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Een fout bestand is doorgegeven. Probeer opnieuw.");
                    }
                }
                else
                {
                    throw new ArgumentException("Abilities.csv was leeg");
                }
            }
        }
        public static List<Ability> GetAbilitiesByChampionName(string championName)
        {
            List<Ability> abilitiesChamp = Abilities.Where(x => x.ChampionName == championName).ToList();
            return abilitiesChamp;
        }
    }
}
