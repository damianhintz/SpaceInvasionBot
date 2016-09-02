using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SpaceInvasionBot
{
	enum JobType
	{
		Order,
		Building,
		Research,
		Fleet,
		Defence
	}
	
	class CommanderEventArgs : EventArgs
	{
		//public JobType Job;
	}

	/// <summary>
	/// Komandor planet.
	/// </summary>
	class Commander
	{
		public delegate void CommanderEventHandler(object sender, CommanderEventArgs e);
		
		//Events
		public event CommanderEventHandler CommanderHandler;

        protected virtual void OnCommander(CommanderEventArgs e)
        {
            if (CommanderHandler != null)
            {
                CommanderHandler(this, e);
            }
        }
		//Fleet return/send
		//Building build
		//Research done
		//Fleet/defence build
		
		private Dictionary<string, Planet> _planets;
		private Planet _planet;
		//private int _index;
		
		private Scanner _scanner;
		private Defender _defender;
		private List<Order> _orders;
		private List<FleetMove> _moves;
		
	    public Commander()
	    {
		    //Planets
		    Console.WriteLine("Initializing planets.");
		    string html = Navigator.GoHome();
		    if(html == null)
		    	throw new Exception("Commander died.");
		    if(ParsePlanets(html) == false)
		    	throw new Exception("Commander died. Planets not init.");
		    
		    //Defender
		    _defender = new Defender();
		    
		    //Scanner
		    //_scanner = new Scanner(_planet.Info.Location.Galaxy, _planet.Info.Location.System);
		    _scanner = new Scanner(2, 2, 119, 119);
		    //_scanner.Start();
		    
	    }
	    
        public void Start()
        {
      		Console.WriteLine("Commander started.");
        }
        
        public void CheckPlanet()
        {
	        if(_scanner.IsOn == false)
	        {
		        Console.WriteLine("Spy report");
		        //Spy.SearchPlanets();
	        }
	        
	        if(_defender.Active)
	        {
		        _defender.CheckAttack();
	        }
	        
	        string html = null;
	        //string html = Navigator.GoOrder();
	        
			if(html != null)
			{
				
				_orders = Order.Parse(html);
				foreach(Order order in _orders)
					;//ObjectWriter.Write(order.Fleet);
			}
        	
	        //UpdatePlanets();
	        //_planet.UpdateWeapon();
	        
	        /*if(_planet.Fleet.SpyProbe > 0)
	        {
		        if(Spy.Send(8, 56, 12))
		        	Console.WriteLine("Spy send.");
		        else
		        	Console.WriteLine("Spy not send.");
        	}*/
        }
        
        private void UpdatePlanets()
        {
	        foreach(KeyValuePair<string, Planet> kvp in _planets)
	        {
		        string pid = kvp.Key;
		        ChangePlanet(pid);
	        }
        }
        
		private string ParsePlanet(string html)
		{
			//Home&sid=[0-9a-f]{32}&
			string pattern = @"newPlanet=(?<planet>[0-9]+)"" SELECTED";
			Regex regex = new Regex(pattern);
			if(regex.IsMatch(html))
			{
				Match match = regex.Match(html);
				string planetid = match.Groups["planet"].ToString();
				return planetid;
			}
			return null;
		}
		
		private bool ParsePlanets(string html)
		{
			_planets = new Dictionary<string, Planet>();
			//Home&sid=[0-9a-f]{32}&
			string pattern = @"newPlanet=(?<planet>[0-9]+)";
			Regex regex = new Regex(pattern);
			if(regex.IsMatch(html))
			{
				MatchCollection matches = regex.Matches(html);
				Console.WriteLine("Parsed planets ({0}).", matches.Count);
				foreach(Match match in matches)
				{
					string planetid = match.Groups["planet"].ToString();
					Planet planet = new Planet(planetid);
					_planets.Add(planetid, planet);
				}
			}else
				return false;
			string pid = ParsePlanet(html);
			if(pid != null)
			{
				Console.WriteLine("Main planet ({0})", pid);
				_planet = _planets[pid];
			}
			else
				Console.WriteLine("Selected planet not found.");
			return true;
		}
		
        private bool ChangePlanet(string planetid)
        {
	        Console.WriteLine("Changing main planet {0} to {1} ...", _planet.Id, planetid);
	        string url = Navigator.GetHome() + "&newPlanet=" + planetid;
	        string html = Navigator.Go(url, null);
	        if(html != null)
	        {
		        _planet = _planets[planetid];
		        Console.WriteLine("Main planet changed.");
		        return true;
	        }
	        Console.WriteLine("Change error.");
	        return false;
        }
        
        public void Build()
        {
	        //(get)http://inwazja.wp.pl/indexInternal.es?action=internalBuildItem&buildingID=14&sid=0a6bbe89f85f98e45f9e5ae9821614ed
	        //(get)http://inwazja.wp.pl/indexInternal.es?action=internalBuildItem&researchID=14&sid=0a6bbe89f85f98e45f9e5ae9821614ed
	        //(post)indexInternal.es?action=internalBuildItem&buildShip=1
	        //(post)indexInternal.es?action=internalBuildItem&buildDefense=1
        }
	}
}