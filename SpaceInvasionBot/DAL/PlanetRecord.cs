using System;
using System.Data;
using System.Data.OleDb;

namespace SpaceInvasionBot.DAL
{
    class PlanetRecord : TableRecord
    {
        public string Name = "";
        public int Galaxy = 0;
        public int System = 0;
        public int Planet = 0;
        public int Metal = 0;
        public int Kryptonite = 0;
        
        public UserRecord GetUser()
        {
	        UserRecord user = new UserRecord { Id = this.Id };
	        return (UserRecord)user.Select();
        }
        
        public ReportRecord GetReport()
        {
	        ReportRecord report = new ReportRecord{ Id = this.Id };
	        return (ReportRecord)report.Select();
        }
    }
}
