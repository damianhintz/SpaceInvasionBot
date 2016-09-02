using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceInvasionBot
{
    class Report
    {
        public DateTime LastUpdate = DateTime.Now;
        
        public Resource Resource = null;
        public Building Building = null;
        public Research Research = null;
        public Defence Defence = null;
        public Fleet Fleet = null;
        
        public static Report Parse(string html)
		{
			Report report = new Report();
			try
			{
				if(html.Contains("Nie znaleziono"))
				{
					return null;
				}
				report.LastUpdate = ParseDate(html);
				
				report.Resource = ReportResourceParser.ParseResource(html);
				report.Building = ReportBuildingParser.ParseBuilding(html);
				report.Research = ReportResearchParser.ParseResearch(html);
				report.Fleet = ReportFleetParser.ParseFleet(html);
				report.Defence = ReportDefenceParser.ParseDefence(html);
				
			}catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return report;
		}
		
		public static DateTime ParseDate(string html)
        {
	        string pattern = @"(?<year>[0-9]+)\-(?<month>[0-9]+)\-(?<day>[0-9]+)\s+(?<hour>[0-9]+):(?<minute>[0-9]+):(?<second>[0-9]+)";
	        Regex regex = new Regex(pattern);
	        if(regex.IsMatch(html))
	        {
		        Match match = regex.Match(html);
		        int year = int.Parse(match.Groups["year"].ToString());
		        int month = int.Parse(match.Groups["month"].ToString());
		        int day = int.Parse(match.Groups["day"].ToString());
		        int hour = int.Parse(match.Groups["hour"].ToString());
		        int minute = int.Parse(match.Groups["minute"].ToString());
		        int second = int.Parse(match.Groups["second"].ToString());
		        return new DateTime(year, month, day, hour, minute, second);
	        }
	        return DateTime.MinValue;
        }
    }
}
