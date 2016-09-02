using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvasionBot
{	
    class InternalResearchParser : ResearchParser
    {
        public InternalResearchParser(Research research) : base(research)
        {
	        //>Technologia kopalni</a> (Poziom 19)
            _pattern = @">{0}</a> \(Poziom\s+(?<level>[0-9]+)\)";
			_parser = new ObjectParser
			{
				Object = _research,
				Pattern = _pattern,
				Patterns = ResearchParser.Patterns
			};
        }
    }
}
