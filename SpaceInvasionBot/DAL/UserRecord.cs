using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace SpaceInvasionBot.DAL
{
    class UserRecord : TableRecord
    {
        public string Name = "";
        public int Points = 0;
        public int Alliance = 0;
        public string Status = "";
        
        public AllianceRecord GetAlliance()
        {
	        AllianceRecord alliance = new AllianceRecord { Id = Alliance };
	        return (AllianceRecord)alliance.Select();
        }
        
        public List<UserRecord> GetAll()
        {
	        List<TableRecord> tusers = this.SelectAll();
	        List<UserRecord> users = tusers.ConvertAll(
	        	new Converter<TableRecord, UserRecord>(
	        		delegate(TableRecord t)
	        		{
		        		return (UserRecord)t;
		        	}
	        	)
	        );
	        return users;
        }
    }
}
