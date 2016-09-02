using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceInvasionBot
{
	enum MissionType
	{
		Colonize,
		Transport,
		Spionage,
		Attack,
		Safe
	}
	
	class Order
	{
		/*public OrderType Type;
		public string pid1, pid2;
		public string uid;*/
		public string Start;
		public string End;
		public string CzasStart;
		public string CzasEnd;
		public string Mission;
		public Fleet Fleet;
		
		public Order()
		{
			Fleet = new Fleet();
		}
		
		public static List<Order> Parse(string html)
		{
		//<td class="nachricht" width="100">
		//<a href="#" class="bau" onmouseover="this.T_TITLECOLOR='#000000';this.T_BORDERCOLOR='#E6EFFF';this.T_TITLE='Zestawienie floty';
		//this.T_LEFT=1;this.T_WIDTH=200;this.T_BGCOLOR='#EDEDED';
		//return escape('1 Transmiter<br>1 Sonda szpiegowska<br>1 Recycler<br>')">3 Jednostka(i) (H)</a>
			
			List<Order> orders = new List<Order>();
			string pattern = 
			@"<td class=""nachricht"" width=""100"">\s*<a href[^\(]+\('(?<fleet>[^']*)'[^<]+</a>[^<]+</td>\s*" +
		    @"<td class=""nachricht"" width=""100"">\s*(?<mission>[^<]+)</td>\s*" +
		    @"<td class=""nachricht"" width=""100"">\s*(?<start>[^<]+)</td>\s*" +
		    @"<td class=""nachricht"" width=""120"">\s*(?<begin>[^<]+)</td>\s*" +
		    @"<td class=""nachricht"" width=""100"">\s*(?<cel>[^<]+)</td>\s*" +
		    @"<td class=""nachricht"" width=""120"">\s*(?<end>[^<]+)</td>";
		    Regex regex = new Regex(pattern);
		    if(regex.IsMatch(html))
		    {
			    foreach(Match match in regex.Matches(html))
			    {
					Order order = new Order();
					
					string fleet = match.Groups["fleet"].ToString().Trim();
					OrderFleetParser parser = new OrderFleetParser(order.Fleet);
					parser.Parse(fleet);
					
				    order.Mission = match.Groups["mission"].ToString().Trim();
				    order.Start = match.Groups["start"].ToString().Trim();
				    order.End = match.Groups["cel"].ToString().Trim();
				    order.CzasStart = match.Groups["begin"].ToString().Trim();
				    order.CzasEnd = match.Groups["end"].ToString().Trim();
				    orders.Add(order);
			    }
		    }
		    return orders;
		}
	}
	
	
}