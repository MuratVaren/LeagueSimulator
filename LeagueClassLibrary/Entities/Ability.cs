using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueClassLibrary.Entities
{
    public class Ability
    {
        public int Id { get; set; }
        public string ChampionName { get; set; }
        public string Name { get; set; }
        public Ability(int id, string champName, string abilityName)
        {
            Id = id;
            ChampionName = champName;
            Name = abilityName;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
