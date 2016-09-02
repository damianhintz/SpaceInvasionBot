using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvasionBot
{
    class InternalDefenceParser : DefenceParser
    {
        public InternalDefenceParser(Defence defence) : base(defence)
        {
            _pattern = @">{0}</a> \(dost.pne:\s+<b>(?<level>[0-9]+)</b>";
            _parser = new ObjectParser
			{
				Object = _defence,
				Pattern = _pattern,
				Patterns = DefenceParser.Patterns
			};
        }
    }
}
