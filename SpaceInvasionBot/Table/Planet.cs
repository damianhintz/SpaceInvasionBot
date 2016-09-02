using System;
using SpaceInvasionBot.DAL;

namespace SpaceInvasionBot
{
	class Planet
	{
		public string Id = "";
		public string Name = "";
        public int Galaxy = 0;
        public int System = 0;
        public int Number = 0;
        public int Metal = 0;
        public int Kryptonite = 0;
        
		public User User = null;
		public Report Report = null;
		
		private InternalOrderParser _orderParser;
		
		private Resource _resource;
		
		private Fleet _fleet;
		private InternalWeaponParser _weaponParser;
		
		private Defence _defence;
		private InternalDefenceParser _defenceParser;
		
		private Building _building;
		private InternalBuildingParser _buildingParser;
		
		private Research _research;
		private InternalResearchParser _researchParser;
		
		public Planet()
		{
		    _building = new Building();
		    _buildingParser = new InternalBuildingParser(_building);
		    
		    _research = new Research();
		    _researchParser = new InternalResearchParser(_research);
		    
		    _fleet = new Fleet();
		    _orderParser = new InternalOrderParser(_fleet);
		    _weaponParser = new InternalWeaponParser(_fleet);
		    
		    _defence = new Defence();
		    _defenceParser = new InternalDefenceParser(_defence);
		}
		
		public Planet(string id) : this()
		{
			Id = id;
		}
		
		public int Distance(Planet p)
		{
			int v1 = 100 * Galaxy + 10 * System + Number;
			int v2 = 100 * p.Galaxy + 10 * p.System + p.Number;
			return Math.Abs(v1 - v2);
		}
		
		public void UpdateReport()
		{
			string url = Navigator.GetSpionage() + "&planetID=" + Id;
        	string html = Navigator.Go(url, null);
        	if(html == null)
        		return ;
        	Report = Report.Parse(html);
		}
		
		public void UpdatePlanet()
		{
			Refresh();
			//ResourceRecord rec = new ResourceRecord { Id = this.Id };
			//ObjectDumper.Dump(_resource, rec);
			//_resource.ToDB(rec);
		}
		
		public void Refresh()
		{
			RefreshResource();
			RefreshBuilding();
			RefreshResearch();
			RefreshWeapon();
			RefreshDefence();
		}
		
		public void RefreshResource()
		{
			string html = Navigator.GoHome();
			if(html != null) _resource = InternalResourceParser.ParseResource(html);
		}
		
		public void RefreshBuilding()
		{
			string html;
			html = Navigator.GoBuilding();
			if(html != null) _buildingParser.Parse(html);
		}
		
		public void RefreshWeapon()
		{
			string html;
			html = Navigator.GoWeapon();
			if(html != null) _weaponParser.Parse(html);
		}
		
		public void RefreshResearch()
		{
			string html;
			html = Navigator.GoResearch();
			if(html != null) _researchParser.Parse(html);
		}
		
		public void RefreshDefence()
		{
			string html;
			html = Navigator.GoDefence();
			if(html != null) _defenceParser.Parse(html);
		}
	}
}