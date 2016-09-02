using System;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SpaceInvasionBot
{
    class ObjectParser
    {
	    private object _object;
	    public object Object
	    {
		    get { return _object; }
		    set { _object = value; }
	    }
        private string _pattern;
        public string Pattern
        {
	        get { return _pattern; }
	        set { _pattern = value; }
        }
        private string[] _patterns;
        public string[] Patterns
        {
	        get { return _patterns; }
	        set
	        {
		        _patterns = value;
		        if(_object.GetType().GetFields().Length != _patterns.Length)
	        		throw new Exception("Number of patterns don't match number of fields of the object.");
			}
        }

        public ObjectParser()
        {
	        _object = null;
	        _pattern = null;
	        _patterns = null;
        }
        
        public ObjectParser(object obj)
        {
	        _object = obj;
        }
        
        private string BuildPattern(string name)
        {
            return string.Format(_pattern, name);
        }

        public object Parse(string text)
        {
	        Type type = _object.GetType();
            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < _patterns.Length; i++)
            {
	            type.InvokeMember(fields[i].Name, BindingFlags.SetField, null, _object, new object[] { 0 });
                string pattern = _patterns[i];
                Regex regex = new Regex(BuildPattern(pattern));
                if (regex.IsMatch(text))
                {
                    Match match = regex.Match(text);
                    int val = int.Parse(match.Groups["level"].ToString());
                    type.InvokeMember(fields[i].Name, BindingFlags.SetField, null, _object, new object[] { val });
                }
            }
            return _object;
        }

        public virtual void Write()
        {
            foreach (FieldInfo field in _object.GetType().GetFields())
            {
                Console.WriteLine("{0}={1}", field.Name, field.GetValue(_object));
            }
        }
    }
}
