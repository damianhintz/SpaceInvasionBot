using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvasionBot
{
    class ReportDefenceParser : DefenceParser
    {
        public ReportDefenceParser(Defence defence) : base(defence)
        {
            _pattern = @"<td class=nachricht width=120>{0}</td>\s*<td width=20 class=nachricht>(?<level>[^<]+)</td>";
            _parser = new ObjectParser
			{
				Object = _defence,
				Pattern = _pattern,
				Patterns = DefenceParser.Patterns
			};
        }
        
		public static Defence ParseDefence(string html)
		{
			Defence defence = new Defence();
			ReportDefenceParser parser = new ReportDefenceParser(defence);
			parser.Parse(html);
			return defence;
		}
    }
}
