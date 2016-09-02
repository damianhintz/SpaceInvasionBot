using System;
using System.Text;

namespace SpaceInvasionBot
{
	abstract class FleetParser
	{
		protected Fleet _fleet;
		protected ObjectParser _parser;
		protected string _pattern;
		
		public readonly static string[] Patterns = new string[]
		{
			"Ma.y transporter", "Du.y transporter", "Transmiter",
			"Ma.y my.liwiec", "Du.y my.liwiec", "Gwiezdny kr..ownik", "Phoenix",
			"Statek kolonialny", "Ogniwo s.oneczne", "Sonda szpiegowska", "Recycler", 
			"Statek wojenny", "Bombowiec z p.aszczem maskuj.cym", "Niszczyciel", "Imperialna gwiezdna baza"
		};
            
		protected FleetParser(Fleet fleet)
		{
			_fleet = fleet;
			_pattern = "";
		}
		
		public Fleet Parse(string html)
		{
			return (Fleet)_parser.Parse(html);
		}
	}
}
