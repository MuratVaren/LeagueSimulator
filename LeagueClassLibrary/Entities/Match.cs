using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassLibrary.Entities
{
    public abstract class Match: IWinnable
    {
        public List<Champion> Team1Champions { get; set; }
        public List<Champion> Team2Champions { get; set; }
        public int Winner { get; set; }
        public string Code { get; set; }
        public Match(string code)
        {
            Code = code;
        }
        public abstract void GenereerTeams();
        public void DecideWinner()
        {
            var gemTeam1 =
                Team1Champions.Average(champ => champ.ReleaseYear);
            var gemTeam2 =
                Team2Champions.Select(champ => champ.ReleaseYear).Average();
            if(gemTeam1 > gemTeam2)
            {
                Winner = 1;
            }
            else if(gemTeam1 < gemTeam2)
            {
                Winner = 2;
            }
            else
            {
                int team1AssassinCount = Team1Champions.Count(champ => champ.Class.ToUpper() == "ASSASSIN");
                int team2AssassinCount = Team2Champions.Count(champ => champ.Class.ToUpper() == "ASSASSIN");
                if (team1AssassinCount > team2AssassinCount)
                {
                    Winner = 1;
                }
                else if(team1AssassinCount < team2AssassinCount)
                {
                    Winner = 2;
                }
                else
                {
                    Winner = 1;
                }
            }
        }

    }
}
