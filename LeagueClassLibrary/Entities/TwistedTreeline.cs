using LeagueClassLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassLibrary.Entities
{
    public class TwistedTreeline: Match
    {
        public TwistedTreeline(string code) : base(code)
        {
            Code = code;
            Team1Champions = new List<Champion>();
            Team2Champions = new List<Champion>();
        }
        public override void GenereerTeams()
        {
            string[] positions = { "top", "top", "jung" };
            foreach(string pos in positions)
            {
                Team1Champions.Add(ChampionData.GetRandomChampion(pos));
                Team2Champions.Add(ChampionData.GetRandomChampion(pos));
            }
        }
    }
}
