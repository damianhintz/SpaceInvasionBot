using System;

namespace SpaceInvasionBot.DAL
{
    class ReportRecord : TableRecord
    {
        //private bool _destroyed;
        public string Status = "";
        public DateTime LastUpdate = DateTime.Now;

        //public int Resource = 0;
        //public int Building = 0;
        //public int Research = 0;
        //public int Defence = 0;
        //public int Fleet = 0;
        
        public PlanetRecord GetPlanet()
        {
	        PlanetRecord planet = new PlanetRecord{ Id = this.Id };
	        return (PlanetRecord)planet.Select();
        }
        
        public ResourceRecord GetResource()
        {
	        ResourceRecord resource = new ResourceRecord{ Id = this.Id };
	        return (ResourceRecord)resource.Select();
        }
    }
}
