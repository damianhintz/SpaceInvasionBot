using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvasionBot
{
	class ReportResearchParser : ResearchParser
    {
        public ReportResearchParser(Research research) : base(research)
        {
            _pattern = @"<td class=nachricht width=120>{0}</td>\s*" +
                       @"<td width=20 class=nachricht>(?<level>[^<]+)</td>";
            _parser = new ObjectParser
			{
				Object = _research,
				Pattern = _pattern,
				Patterns = ResearchParser.Patterns
			};
        }
        
		public static Research ParseResearch(string html)
		{
			Research research = new Research();
			ReportResearchParser parser = new ReportResearchParser(research);
			parser.Parse(html);
			return research;
		}
    }
}
