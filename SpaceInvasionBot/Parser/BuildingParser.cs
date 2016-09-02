using System;
using System.Text;

namespace SpaceInvasionBot
{
	abstract class BuildingParser
	{
		protected Building _building;
		protected ObjectParser _parser;
		protected string _pattern;
		
		public readonly static string[] Patterns = new string[]
		{
			@"Kopalnia .elaza", @"Magazyn .elaza", "Piec hutniczy", "Magazyn metalu",
            "Kopalnia kryptonitu", "Magazyn kryptonitu", "Kopalnia spice", "Magazyn spice",
            "Elektrownia fuzyjna", "Fabryka termosolarna", "Centrum rozwojowe", "Laboratorium badawcze",
            "Fabryka broni", "Baza floty", @"Os.ona cz.steczkowa", @"Dzia.o cz.steczkowe",
            "Przyspieszacz mikrosystemowy", "Teleporter", "Skaner galaktyczny"
		};
            
		protected BuildingParser(Building building)
		{
			_building = building;
			_pattern = "";
		}
		
		public Building Parse(string html)
		{
			return (Building)_parser.Parse(html);
		}
	}
}
