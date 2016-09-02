using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Linq;
using SpaceInvasionBot.DAL;

namespace SpaceInvasionBot
{
	///
	/// 1. Wybierz nieaktywne planety.
	///	2. Wybierz planetê z której wys³aæ sondê.
	///
	static class Spy
	{
		public static Dictionary<string, Planet> Planets;
		
		///
		/// Szukaj planet nieaktywnych u¿ytkowników, dla których nie ma aktualnego raportu
		///
		public static void SearchPlanets()
		{
			Planets = new Dictionary<string, Planet>();
			var q = from planet in Scanner.Planets where planet.Value.User.Status == "Nieaktywny" && planet.Value.Report == null select planet.Value;
			foreach(Planet p in q)
			{
				Planets[p.Id] = p;
			}
		}
		
		///
		/// Wybierz planetê najbli¿sz¹ planecie wyjœciowej
		///
		public static Planet SelectPlanet(Planet home)
		{
			int min = int.MaxValue;
			Planet pmin = null;
			foreach(KeyValuePair<string, Planet> kv in Planets)
			{
				Planet p = kv.Value;
				if(home.Distance(p) < min)
				{
					min = home.Distance(p);
					pmin = p;
				}
			}
			Planets.Remove(pmin.Id);
			return pmin;
		}
		
		public static bool Send(string pid)
		{
			return true;
		}
		
		public static bool Send(int g, int s, int p)
		{
			string html = Navigator.GoOrder();
			if(html == null) return false;
            
			string sid = Navigator.GetValue(html, "sid");
            string reloadToken = Navigator.GetValue(html, "reloadToken");
            string data = string.Format("sid={0}&reloadToken={1}&planet=::&gal={2}&sun={3}&pla={4}&sendFleet=1&have_1=1&ship_10=1", 
            	sid, reloadToken, g, s, p);
            html = Navigator.GoSend(data);
            if(html.Contains("Zestawienie floty"))
            {
	            Console.WriteLine("(OK) Zestawienie floty.");
            }else
            {
            	Console.WriteLine("(NO) Zestawienie floty.");
            	return false;
        	}
            
            string distance = Navigator.GetValue(html, "distance");
        	string capacity = Navigator.GetValue(html, "capacity");
        	string target = Navigator.GetValue(html, "targetPlanetID");
        	string speed = Navigator.GetValue(html, "lowestSpeed");
        	
            data = string.Format("sid={0}&reloadToken={1}&destinationType=PLANET&todo=SPIONAGE&speed=100&" +
            	"fleet_roh=&fleet_met=&fleet_kry=&fleet_spi=&" +
            	"distance={2}&capacity={3}&targetPlanetID={4}&lowestSpeed={5}&" +
            	"ship_10=1", sid, reloadToken, distance, capacity, target, speed);
            html = Navigator.GoFinish(data);
            
            if(html == null)
            	return false;
            if(html.Contains("Twoja flota zosta"))
            	return true;
            return false;
		}
	}
}