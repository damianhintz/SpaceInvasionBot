using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SpaceInvasionBot.DAL;

namespace SpaceInvasionBot
{
	class Scanner
	{
        private int _galaxy = 2;
        private int _system = 119;
        
        private bool _scanend;
        public bool IsOn
        {
	        get { return _scanend == false; }
        }

        public static int MinGalaxy = 1;
        public static int MinSystem = 1;
        public static int MinPlanet = 1;
        public static int MaxGalaxy = 14;
        public static int MaxSystem = 400;
        public static int MaxPlanet = 16;

        private int _firstGalaxy;
        public int FirstGalaxy
        {
	        get { return _firstGalaxy; }
	        set { _firstGalaxy = value > MinGalaxy && value < MaxGalaxy && value <= _lastGalaxy ? value : MinGalaxy; }
        }
        private int _lastGalaxy;
        public int LastGalaxy
        {
	        get { return _lastGalaxy; }
	        set { _lastGalaxy = value > MinGalaxy && value < MaxGalaxy  && value >= _firstGalaxy ? value : MaxGalaxy; }
        }
        private int _firstSystem;
        public int FirstSystem
        {
	        get { return _firstSystem; }
	        set { _firstSystem = value > MinSystem && value < MaxSystem && value <= _lastSystem ? value : MinSystem; }
        }
        private int _lastSystem;
        public int LastSystem
        {
	        get { return _lastSystem; }
	        set { _lastSystem = value > MinSystem && value < MaxSystem  && value >= _firstSystem ? value : MaxSystem; }
        }

        public static Dictionary<string, User> Users = new Dictionary<string, User>();
        public static Dictionary<string, Alliance> Alliances = new Dictionary<string, Alliance>();
        public static Dictionary<string, Planet> Planets = new Dictionary<string, Planet>();
        //public Dictionary<string, Report> Reports = new Dictionary<string, Report>();
        
        public Scanner()
        {
	        _firstGalaxy = MinGalaxy;
	        _firstSystem = MinSystem;
	        _lastGalaxy = MaxGalaxy;
	        _lastSystem = MaxSystem;
        }

        public Scanner(int firstGalaxy, int lastGalaxy, int firstSystem, int lastSystem)
        {
	        _firstGalaxy = firstGalaxy;
	        _firstSystem = firstSystem;
	        _lastGalaxy = lastGalaxy;
	        _lastSystem = lastSystem;
        }
        
        public bool Start()
        {	        	
      		_scanend = false;
      		_galaxy = _firstGalaxy;
            _system = _firstSystem;
         
      		Console.WriteLine("Scanner started.");
      		return true;
  		}
  		
  		public void Scan()
        {        
	        if(_scanend)
		        return ;
	        
		    string url = Navigator.GetGalaxy();
	        string html = Navigator.Go(string.Format(url + "&g={0}&s={1}", _galaxy, _system), null);
	        
            ParseSystem(html);
            _scanend = Next();
        }
        
        private bool Next()
        {
	        if(++_system > _lastSystem)
            {
                _system = _firstSystem;
                if(++_galaxy > _lastGalaxy)
                {
                    _scanend = true;
                    _galaxy = _firstGalaxy;
                    Console.WriteLine("Scan end.");
                    return true;
                }
            }
            return false;
        }
        
        private void ParseSystem(string text)
        {
	        Console.WriteLine("Parsing system {0} ...", _system);
            string[] planets = text.Split(new string[] { "*|*|*" }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
	            foreach (string pl in planets)
	            {
	                string[] values = pl.Split(new string[] { "*|*" }, StringSplitOptions.RemoveEmptyEntries);
	                if (values.Length == 16)
	                {
	                    string pnr = values[4].Trim(), pname = values[7].Trim(), ploc = values[8], info = values[9], uname = "", status = "", aname = "";
	                    string pid = values[15].Trim(), uid = "", aid = "";
	                    int points = 0;
	                    
	                    Alliance a = null;
	                    User u = null;
	                    Planet p = new Planet{ Id = pid, Name = pname.Trim(), User = null, Report = null };
	                    //Report r = null;
	                    
	                    if (!pname.Contains("Niezamieszkane"))
	                    {
		                    if(ParseAlliance(info, out aid, out aname))
		                    {
		                    	//AllianceRecord alliance = new AllianceRecord{ Id = int.Parse(aid), Name = aname.Trim() };
		                    	//if(alliance.Select() == null) alliance.Insert();
		                    	a = new Alliance { Id = aid, Name = aname };
		                    	if(!Alliances.ContainsKey(aid))
		                    		Alliances.Add(aid, a);
	                    		else
	                    			Alliances[aid] = a;
	                    	}
	                        if(ParseUser(info, out uid, out uname))
	                        {
		                        UserRecord user = new UserRecord();
		                        ParsePoints(info, out points);
	                        	ParseStatus(info, out status);
	                        	//user.Id = int.Parse(uid);
	                        	user.Name = uname.Trim();
	                        	user.Points = points;
	                        	user.Status = status.Trim();
	                        	//user.Alliance = int.Parse(aid);
	                        	//ObjectDumper.Dump(user);
	                        	u = new User{ Id = uid };
	                        	u.Name = uname.Trim(); u.Points = points; u.Status = status.Trim(); u.Alliance = a;
	                        	if(Users.ContainsKey(uid))
	                        		Users[uid] = u;
	                        	else
		                        	Users.Add(uid, u);
		                        p.User = u;
	                        }
	                    }
	                    else pname = "";
	                    
	                    
	                    int galaxy = 0, system = 0, nr = 0;
	                    ParseLocation(ploc, out galaxy, out system, out nr);
	                    PlanetRecord planet = new PlanetRecord();
	                    //planet.Id = int.Parse(pid);
	                    planet.Name = pname.Trim();
	                    planet.Galaxy = galaxy;
	                    planet.System = system;
	                    planet.Planet = nr;
	                    
	                    p.Galaxy = galaxy; p.System = system; p.Number = nr; p.Name = pname.Trim();
	                    
	                    //Planet p = new Planet { Id = pid, Name = pname.Trim(), Galaxy = galaxy, System = system, Number = nr };
	                    //Console.WriteLine("planet {0}", pid);
	            		//ReportRecord report = new ReportRecord { Id = int.Parse(pid) };
	            		//ParseReport(html, report);
	            		/*if(report != null)
	            		{
	                		report.Id = int.Parse(pid);
	                		//ObjectDumper.Dump(report);
	            		}else
	            			Console.WriteLine("Brak raportu.");
						*/
	                    //ObjectDumper.Dump(planet);
						//if(Planets.ContainsKey(pid))
						
						p.UpdateReport();
						Planets[pid] = p;
						//u.Planets[pid] = p;	
	                }
	            }
        	}catch(Exception ex)
        	{
	        	Console.WriteLine(ex.ToString());
        	}
        }

        private bool ParseUser(string info, out string id, out string name)
        {
            id = ""; name = null;
            string pattern = @"sendMessage\((?<id>\d+)\)"">(?<name>[^ ]+) <img";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(info))
            {
                Match match = regex.Match(info);
                id = match.Groups["id"].ToString();
                name = match.Groups["name"].ToString();
                return true;
            }
            return false;
        }

        private bool ParsePoints(string info, out int points)
        {
            points = 0;
            string pattern = @"Punkty[^&]*&nbsp;(?<points>[0-9\.]+)";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(info))
            {
                Match match = regex.Match(info);
                points = int.Parse(match.Groups["points"].ToString().Replace(".", ""));
                return true;
            }
            return false;
        }

        private bool ParseLocation(string info, out int galaxy, out int system, out int planet)
        {
	        galaxy = system = planet = 0;
            string pattern = @"(?<galaxy>\d+):(?<system>\d+):(?<planet>\d+)";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(info))
            {
                Match match = regex.Match(info);
                galaxy = int.Parse(match.Groups["galaxy"].ToString());
                system = int.Parse(match.Groups["system"].ToString());
                planet = int.Parse(match.Groups["planet"].ToString());
                return true;
            }
            return false;
        }

        private bool ParseAlliance(string info, out string id, out string name)
        {
	        id = "";
	        name = "";
            string pattern = @"allianceID=(?<id>\d+)[^>]+>(?<name>[^<]+)<";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(info))
            {
                Match match = regex.Match(info);
                id = match.Groups["id"].ToString();
                name = match.Groups["name"].ToString();
                return true;
            }
            return false;
        }

        private bool ParseStatus(string info, out string status)
        {
            status = "";
            string pattern = @">(?<status>[^<]+)</font></b>";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(info))
            {
                Match match = regex.Match(info);
                status = match.Groups["status"].ToString();
                return true;
            }
            return false;
        }

        private bool ParseRubberFields(string info, out int metal, out int kryptonite)
        {
			kryptonite = 0;
			metal = 0;
			string pattern = @"<tr>Metal</tr> <tr>Kryptonit>\s+(?<kryptonite>[0-9\.]+)</tr>";
			Regex regex = new Regex(pattern);
			if (regex.IsMatch(info))
			{
			    Match match = regex.Match(info);
			    metal = int.Parse(match.Groups["metal"].ToString().Replace(".", ""));
			    kryptonite = int.Parse(match.Groups["kryptonite"].ToString().Replace(".", ""));
			    return true;
			}
			return false;
        }
        
		public static bool ParseReport(string html, ReportRecord report)
		{
			try
			{
				if(html.Contains("Nie znaleziono"))
				{
					report.Status = "Brak";
					report.LastUpdate = DateTime.Now;
				}
				else
				{
					report.Status = "Jest";
					//report.LastUpdate = ParseDate(html);
				}
				
				ResourceRecord resource = new ResourceRecord { Id = report.Id };
				BuildingRecord building = new BuildingRecord { Id = report.Id };
				ResearchRecord research = new ResearchRecord { Id = report.Id };
				DefenceRecord defence = new DefenceRecord { Id = report.Id };
				FleetRecord fleet = new FleetRecord { Id = report.Id };
				
				Resource r = ReportResourceParser.ParseResource(html);
				if(r != null)
					r.ToDB(resource);
				Building b = ReportBuildingParser.ParseBuilding(html);
				if(b != null)
					ObjectDumper.Dump(b, building);
				Research s = ReportResearchParser.ParseResearch(html);
				if(s != null)
					ObjectDumper.Dump(s, research);
				Fleet f = ReportFleetParser.ParseFleet(html);
				ObjectDumper.Dump(f, fleet);
				
				Defence d = ReportDefenceParser.ParseDefence(html);
				ObjectDumper.Dump(d, defence);
			}catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			return true;
		}
    }
}