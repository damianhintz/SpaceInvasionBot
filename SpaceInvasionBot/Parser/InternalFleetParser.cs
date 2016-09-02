using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvasionBot
{
    class InternalWeaponParser : FleetParser
    {
        public InternalWeaponParser(Fleet fleet) : base(fleet)
        {
	        //>Mo�dzierz</a> (dost�pne: <b>2</b>, mo�liwe: <b>0</b>)
            _pattern = @">{0}</a> \(dost.pne:\s+<b>(?<level>[0-9]+)</b>";
			_parser = new ObjectParser
			{
				Object = _fleet,
				Pattern = _pattern,
				Patterns = FleetParser.Patterns
			};
        }
    }
}
