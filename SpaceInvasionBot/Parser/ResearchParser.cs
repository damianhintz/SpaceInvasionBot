using System;

namespace SpaceInvasionBot
{
    class ResearchParser
    {
	    protected Research _research;
		protected ObjectParser _parser;
		protected string _pattern;
		
		public readonly static string[] Patterns = new string[]
		{
			"Technologia energetyczna", "Technologia grawitacji", "Technologia kopalni", "Technologia os.ony",
			"Systemu celu",	"Badania nap.du", "Mechanizm nap.dowy rakiet",
			"Mechanizm nap.dowy dyfuzji", "Nap.d warp",	"Technologia szpiegowska", "Technologia broni",
			"Systemy obronne", "Analizator cz.steczek",	"Teleporacja", "Badanie czarnej dziury"
		};
		
		protected ResearchParser(Research research)
		{
			_research = research;
			_pattern = "";
		}
		
		public Research Parse(string html)
		{
			return (Research)_parser.Parse(html);
		}
    }
}
