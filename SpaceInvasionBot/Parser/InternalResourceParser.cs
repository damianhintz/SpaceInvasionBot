using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceInvasionBot
{
    class InternalResourceParser
    {
        private static string pattern = @"var ress = new Array\((?<iron>[0-9]+), (?<metal>[[0-9]+), (?<kryptonite>[0-9]+), (?<spice>[0-9]+)";
        private static string energy = @"var energy = new Array\((?<energy>[0-9]+), ";
        private static Regex regex = new Regex(pattern);
        
        public static Resource ParseResource(string html)
        {
            Resource res = null;
            if (regex.IsMatch(html))
            {
                Match match = regex.Match(html);
                res = new Resource();
                res.Iron = int.Parse(match.Groups["iron"].ToString());
                res.Metal = int.Parse(match.Groups["metal"].ToString());
                res.Kryptonite = int.Parse(match.Groups["kryptonite"].ToString());
                res.Spice = int.Parse(match.Groups["spice"].ToString());
                regex = new Regex(energy);
                if(regex.IsMatch(html))
	                res.Energy = int.Parse(regex.Match(html).Groups["energy"].ToString());
            }
            //var max = new Array(1708593.75,1708593.75,1708593.75,1139062.5);
            return res;
        }
    }
}
