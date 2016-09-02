using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceInvasionBot
{
	class FleetMove
	{
		public string Start = "";
		public string End = "";
		public string Mission = "";
		public Fleet Fleet;
		public Resource Resource;
		
		public FleetMove()
		{
			Fleet = new Fleet();
			Resource = new Resource();
		}
		
		public static List<FleetMove> Parse(string html)
		{
		/*moj atak powracajace floty
		<td class=bg_gruen>        Atak            </td>
	    <td class=bg_gruen>	        
	    <a class="bau" onmouseover="this.T_TITLECOLOR='#000000';this.T_BORDERCOLOR='#E6EFFF';this.T_TITLE='Zestawienie floty';this.T_LEFT=1;
	    this.T_WIDTH=200;this.T_BGCOLOR='#EDEDED';return escape('1 Transmiter<br>')">
	    Info</a></td>
	    <td class=bg_gruen>
	    <a class="bau" onmouseover="this.T_TITLECOLOR='#000000';this.T_BORDERCOLOR='#E6EFFF';this.T_TITLE='Towar do transportowania';
	    this.T_LEFT=1;this.T_WIDTH=200;this.T_BGCOLOR='#EDEDED';return escape('Brak')">
	    Info</a></td>
	    <td class=bg_gruen>	    Kolonia [02:61:09]	    </td>
	    <td class=bg_gruen>	        Kolonia [02:60:02]	    </td>
	    <td class=bg_gruen>
	    <a href="#" onclick="sendMessage(4210)">hdmn749 <img src="img/email.gif?__cv=3" width="14" height="10" alt="Wyœlij wiadomoœæ	" border="0" >
	    </a></td>
	    */
	    /*wrogi atak wrogie floty
	    <td class=bg_rot>	        Atak	            </td>
	    <td class=bg_rot>
	        <a class="bau" onmouseover="this.T_TITLECOLOR='#000000';this.T_BORDERCOLOR='#E6EFFF';this.T_TITLE='Zestawienie floty';this.T_LEFT=1;this.T_WIDTH=200;this.T_BGCOLOR='#EDEDED';return escape('6 Transmiter<br>673 Statek wojenny<br>')">Info</a>
	    </td>
	    <td class=bg_rot>	        <a class="bau" >Info</a>	    </td>
	    <td class=bg_rot>	    Galera 1 [02:178:10]	    </td>
	    <td class=bg_rot>	        bob10 [02:179:07]	    </td>
    	*/
	    //return escape('1 Transmiter<br>')">
	    //return escape('55.605 ¯elazo<br>54.163 Metal<br>16.156 Kryptonit<br>66.492 Spice<br>')">
			List<FleetMove> moves = new List<FleetMove>();
			string pattern = 
			@"<td class=bg_gruen>\s*(?<mission>[^<]+)</td>\s*" +
		    @"<td class=bg_gruen>\s*<a class[^\(]+\('(?<fleet>[^']*)'[^<]+</a>\s*</td>\s*" +
		    @"<td class=bg_gruen>\s*<a class[^\(]+\('(?<towar>[^']*)'[^<]+</a>\s*</td>\s*" +
		    @"<td class=bg_gruen>\s*[^\[]+\[(?<start>[^\]]+)\]\s*</td>\s*" +
		    @"<td class=bg_gruen>\s*[^\[]+\[(?<end>[^\]]+)\]\s*</td>\s*";
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
		
		public static List<FleetMove> ParseEnemy(string html)
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