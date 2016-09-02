using System;
using System.Text;

namespace SpaceInvasionBot
{
	class ReportFleetParser : FleetParser
	{
		public ReportFleetParser(Fleet fleet) : base(fleet)
		{
			_pattern = @"<td class=nachricht width=120>{0}</td>\s*<td width=20 class=nachricht>(?<level>[^<]+)</td>";
			_parser = new ObjectParser
			{
				Object = _fleet,
				Pattern = _pattern,
				Patterns = FleetParser.Patterns
			};
		}
		
		public static Fleet ParseFleet(string html)
		{
			Fleet fleet = new Fleet();
			ReportFleetParser parser = new ReportFleetParser(fleet);
			parser.Parse(html);
			return fleet;
		}
	}
}
