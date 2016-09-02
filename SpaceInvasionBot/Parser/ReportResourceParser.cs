using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceInvasionBot
{
    class ReportResourceParser
    {
        private static string pattern = @"<td class=nachricht colspan=""2"">(?<iron>[^<]+)</td>\s*" +
                         @"<td class=nachricht colspan=""2"">(?<metal>[^<]+)</td>\s*" +
                         @"<td class=nachricht colspan=""2"">(?<kryptonite>[^<]+)</td>\s*" +
                         @"<td class=nachricht colspan=""2"">(?<spice>[^<]+)</td>\s*" +
                         @"<td class=nachricht colspan=""2"">(?<energy>[^<]+)</td>\s*";
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
                res.Energy = int.Parse(match.Groups["energy"].ToString());
            }
            return res;
        }
    }
}
