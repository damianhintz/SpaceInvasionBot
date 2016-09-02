using System;
using System.Text;

namespace SpaceInvasionBot
{
	class InternalOrderParser : FleetParser
	{
		public InternalOrderParser(Fleet fleet) : base(fleet)
		{
			_pattern = @"setMax\('ship_{0}', (?<level>[0-9]+)\)";
            string[] _patterns = new string[15];
            for(int i = 0;i < 15;i++)
            	_patterns[i] = (i+1).ToString();
			_parser = new ObjectParser
			{
				Object = _fleet,
				Pattern = _pattern,
				Patterns = _patterns
			};
		}
	}
}
