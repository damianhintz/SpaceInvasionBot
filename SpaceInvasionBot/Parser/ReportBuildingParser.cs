using System;
using System.Text;

namespace SpaceInvasionBot
{	
    class ReportBuildingParser : BuildingParser
    {
        public ReportBuildingParser(Building building) : base(building)
        {
            _pattern = @"<td class=nachricht width=120>{0}</td>\s*<td width=20 class=nachricht>(?<level>[^<]+)</td>";
        	_parser = new ObjectParser
			{
				Object = _building,
				Pattern = _pattern,
				Patterns = BuildingParser.Patterns
			};
        }
        
		public static Building ParseBuilding(string html)
		{
			Building building = new Building();
			ReportBuildingParser parser = new ReportBuildingParser(building);
			parser.Parse(html);
			return building;
		}
    }
}
