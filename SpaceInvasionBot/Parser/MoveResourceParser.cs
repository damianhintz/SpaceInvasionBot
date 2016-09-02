using System;

namespace SpaceInvasionBot
{
    abstract class ResourceParser
    {
	    protected Resource _resource;
		protected ObjectParser _parser;
		protected string _pattern;
		
		public readonly static string[] Patterns = new string[]
		{
			".elazo", "Metal", "Kryptonit", "Spice", "Energia"
		};
            
		protected ResourceParser(Resource resource)
		{
			_resource = resource;
			_pattern = "";
		}
		
		public Resource Parse(string html)
		{
			return (Resource)_parser.Parse(html);
		}
    }
    
    class MoveResourceParser : ResourceParser
    {
        public MoveResourceParser(Resource resource) : base(resource)
        {
            _pattern = @"(?<level>[0-9]+) {0}<br>";
			_parser = new ObjectParser
			{
				Object = _resource,
				Pattern = _pattern,
				Patterns = ResourceParser.Patterns
			};
        }
    }
}
