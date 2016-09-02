using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceInvasionBot
{
    class InternalBuildingParser : BuildingParser
    {
        public InternalBuildingParser(Building building) : base(building)
        {
            _pattern = @">{0}</a> \(Poziom\s+(?<level>[0-9]+)\)";
            _parser = new ObjectParser
			{
				Object = _building,
				Pattern = _pattern,
				Patterns = BuildingParser.Patterns
			};
        }
    }
}
