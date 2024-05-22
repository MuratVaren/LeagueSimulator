using LeagueClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeagueClassLibrary.DataAccess
{
    public static class MatchData
    {
        private static DataTable DataTableMatches {  get; set; }

        public static void InitializeDataTableMatches()
        {
            DataTableMatches = new DataTable("Matches");

            DataColumn dcId = new DataColumn("Id", typeof(int))
            {
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1,
            };
            DataColumn dcCode = new DataColumn("Code", typeof(string));
            DataColumn dcWinner = new DataColumn("Winner",typeof(string));

            DataTableMatches.Columns.Add(dcId);
            DataTableMatches.Columns.Add(dcCode);
            DataTableMatches.Columns.Add(dcWinner);
        }
        public static void AddFinishedMatch(Entities.Match match)
        {
            DataRow row = DataTableMatches.NewRow();
            row["Code"] = match.Code;
            row["Winner"] = match.Winner == 1 ? "Red" : "Blue";
            DataTableMatches.Rows.Add(row);

        }
        public static DataView GetDataViewMatches()
        {
            return new DataView(DataTableMatches);
        }
        public static void ExportToXML(string filepath)
        {
            DataTableMatches.WriteXml(filepath, XmlWriteMode.WriteSchema);
        }
        public static bool IsUniqueCode(string code)
        {
            var codeList = DataTableMatches.AsEnumerable().
                Where(row => row.Field<string>("Code") == code);
            if (codeList.Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
