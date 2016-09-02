using System;

namespace SpaceInvasionBot
{
    class OrderFleetParser : FleetParser
    {
        public OrderFleetParser(Fleet fleet) : base(fleet)
        {
	        //'1 Transmiter<br>1 Sonda szpiegowska<br>1 Recycler<br>'
            _pattern = @"(?<level>[0-9]+) {0}<br>";
			_parser = new ObjectParser
			{
				Object = _fleet,
				Pattern = _pattern,
				Patterns = FleetParser.Patterns
			};
        }
    }
}
