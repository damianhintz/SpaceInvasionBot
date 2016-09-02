using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceInvasionBot
{
	class EnemyFleetMove : FleetMove
	{
		public EnemyFleetMove() : base()
		{
		}
		
		public static List<FleetMove> ParseEnemyT(string html)
		{
		/*wrogi atak wrogie floty
	    <td class=bg_rot>	        Atak	            </td>
	    <td class=bg_rot>
	        <a class="bau" onmouseover="this.T_TITLECOLOR='#000000';this.T_BORDERCOLOR='#E6EFFF';this.T_TITLE='Zestawienie floty';
	        this.T_LEFT=1;this.T_WIDTH=200;this.T_BGCOLOR='#EDEDED';return escape('6 Transmiter<br>673 Statek wojenny<br>')">
	        Info</a>
	    </td>
	    <td class=bg_rot>	        <a class="bau" >Info</a>	    </td>
	    <td class=bg_rot>	    Galera 1 [02:178:10]	    </td>
	    <td class=bg_rot>	        bob10 [02:179:07]	    </td>
    	*/
			List<FleetMove> moves = new List<FleetMove>();
			string pattern = 
			@"<td class=bg_rot>\s*(?<mission>[^<]+)</td>\s*" +
		    @"<td class=bg_rot>\s*<a class[^\(]+\('(?<fleet>[^']*)'[^<]+</a>\s*</td>\s*" +
		    @"<td class=bg_rot>\s*<a class[^>]+>\s*(?<towar>Info)</a>\s*</td>\s*" +
		    @"<td class=bg_rot>\s*[^\[]+\[(?<start>[^\]]+)\]\s*</td>\s*" +
		    @"<td class=bg_rot>\s*[^\[]+\[(?<end>[^\]]+)\]\s*</td>\s*";
		    Regex regex = new Regex(pattern);
		    if(regex.IsMatch(html))
		    {
			    foreach(Match match in regex.Matches(html))
			    {
					FleetMove move = new FleetMove();
					
					string fleet = match.Groups["fleet"].ToString().Trim();
					OrderFleetParser parser = new OrderFleetParser(move.Fleet);
					parser.Parse(fleet);
					
					string resource = match.Groups["towar"].ToString().Trim().Replace(".", "");
					MoveResourceParser parser2 = new MoveResourceParser(move.Resource);
					parser2.Parse(resource);
					
				    move.Mission = match.Groups["mission"].ToString().Trim();
				    move.Start = match.Groups["start"].ToString().Trim();
				    move.End = match.Groups["end"].ToString().Trim().Replace("\n", "");
				    
				    moves.Add(move);
			    }
		    }
		    return moves;
		}
	}
}