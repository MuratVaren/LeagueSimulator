using LeagueClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassLibrary.DataAccess
{
    public static class ChampionData
    {
        private static DataTable DatatableChampions {  get; set; }
        private static Random r = new Random();

        public static void LoadCSV(string padNaarCsv)
        {
            using(StreamReader sr = new StreamReader(padNaarCsv))
            {
                if(!sr.EndOfStream)
                {
                    string headerLine = sr.ReadLine();
                    string[] headerParts = headerLine.Split(';');
                    if(headerParts.Length == 11)
                    {
                        DatatableChampions = new DataTable();
                        foreach (string part in headerParts)
                        {
                            DatatableChampions.Columns.Add(new DataColumn(part, typeof(string)));
                        }
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] parts = line.Split(';');
                            DatatableChampions.Rows.Add(parts);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Een fout bestand is doorgegeven. Probeer opnieuw.");
                    }
                }
                else
                {
                    throw new ArgumentException("ChampionData.csv was leeg");
                }
            }
        }
        public static DataView GetDataViewChampions()
        {
            if(DatatableChampions != null)
            {
                return new DataView(DatatableChampions);
            }
            else
            {
                throw new ArgumentException("Datatable champions is niet ingeladen");
            }
        }
        public static DataView GetDataViewChampionsByPosition(string position)
        {
            if (DatatableChampions != null)
            {
                var filtered = DatatableChampions.AsEnumerable()
                    .Where(row => row.Field<string>("ChampionPosition1") == position ||
                    row.Field<string>("ChampionPosition2") == position ||
                    row.Field<string>("ChampionPosition3") == position);

                DataTable filteredTable = filtered.Any() ?
                    filtered.CopyToDataTable() : DatatableChampions.Clone();

                return new DataView(filteredTable);
            }
            else
            {
                throw new ArgumentException("Datatable champions is niet ingeladen");
            }
        }
        public static DataView GetDataViewChampionsBestToWorst()
        {
            var filtered = DatatableChampions.AsEnumerable()
                .OrderByDescending(row => row.Field<string>("ReleaseYear"))
                .ThenByDescending(row => CountPositions(row))
                .ThenBy(row => row.Field<string>("ChampionName"));

            DataTable filteredTable = filtered.Any() ?
                filtered.CopyToDataTable() : DatatableChampions.Clone();

            return new DataView(filteredTable);
        }
        // Methode om count postions te krijgen
        private static int CountPositions(DataRow row)
        {
            int count = 0;
            if(!string.IsNullOrEmpty(row.Field<string>("ChampionPosition1")))
                count++;
            if (!string.IsNullOrEmpty(row.Field<string>("ChampionPosition2")))
                count++;
            if (!string.IsNullOrEmpty(row.Field<string>("ChampionPosition3")))
                count++;
            return count;
        }
        public static Champion GetRandomChampion(string position)
        {
            DataView dv = GetDataViewChampionsByPosition(position);
            var randomChampion = dv[r.Next(0, dv.Count)].Row;

            List<string> listPositions = new List<string>
            {
                randomChampion.Field<string>("ChampionPosition1")
            };
            if (!string.IsNullOrWhiteSpace(randomChampion.Field<string>("ChampionPosition2")))
            {
                listPositions.Add(randomChampion.Field<string>("ChampionPosition2"));
            }
            if(!string.IsNullOrWhiteSpace(randomChampion.Field<string>("ChampionPosition3")))
            {
                listPositions.Add(randomChampion.Field<string>("ChampionPosition3"));
            }
            Champion champ = new Champion
                (
                randomChampion.Field<string>("ChampionName"),
                randomChampion.Field<string>("ChampionTitle"),
                randomChampion.Field<string>("ChampionClass"),
                int.Parse(randomChampion.Field<string>("ReleaseYear")),
                AbilityData.GetAbilitiesByChampionName(randomChampion.Field<string>("ChampionName")),
                listPositions,
                randomChampion.Field<string>("ChampionIcon"),
                randomChampion.Field<string>("ChampionBanner"),
                int.Parse(randomChampion.Field<string>("ChampionIPCost")),
                int.Parse(randomChampion.Field<string>("ChampionRPCost"))
                );
            return champ;
        }



    }
}
