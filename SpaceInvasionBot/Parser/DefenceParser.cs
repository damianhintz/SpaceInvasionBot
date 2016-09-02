using System;
using System.Text;

namespace SpaceInvasionBot
{
	abstract class DefenceParser
	{
		protected Defence _defence;
		protected ObjectParser _parser;
		protected string _pattern;
		
		public readonly static string[] Patterns = new string[]
		{
			"Mo.dzierz", "Lekkie dzia.o grawitacyjne", "Ci..kie dzia.o grawitacyjne",
			"Dzia.o jonowe", "Turboflak",
			"Pozytron Lighter", "Dzia.o interwa.owe",
			"Bateria dzia.a laserowego", "Dzia.o s.oneczne",
			"Ma.a kopu.a os.onna", "Du.a kopu.a os.onna"
		};
            
		protected DefenceParser(Defence defence)
		{
			_defence = defence;
			_pattern = "";
		}
		
		public Defence Parse(string html)
		{
			return (Defence)_parser.Parse(html);
		}
	}
}
