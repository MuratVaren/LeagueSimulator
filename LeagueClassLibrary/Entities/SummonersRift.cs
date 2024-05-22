using LeagueClassLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassLibrary.Entities
{
    public class SummonersRift : Match
    {
        public SummonersRift(string code) : base(code)
        {
            Code = code;
            Team1Champions = new List<Champion>();
            Team2Champions = new List<Champion>();
        }
        public override void GenereerTeams()
        {
            string[] positions = { "sup", "mid", "jung", "bot", "top" };
            foreach(string pos in positions)
            {
                Team1Champions.Add(ChampionData.GetRandomChampion(pos));
                Team2Champions.Add(ChampionData.GetRandomChampion(pos));
            }
        }
    }
}
