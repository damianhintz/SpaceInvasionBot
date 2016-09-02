using System;
using SpaceInvasionBot.DAL;

namespace SpaceInvasionBot
{
    class Resource
    {
	    /*
	    private int MaxIron;
	    private int MaxMetal;
	    private int MaxKryptonite;
	    private int MaxSpice;
	    */
        public int Iron = 0;
        public int Metal = 0;
        public int Kryptonite = 0;
        public int Spice = 0;
        public int Energy = 0;
        
        public ResourceRecord ToRecord()
        {
	        ResourceRecord resourceRecord = new ResourceRecord();
			ObjectCopy.Copy(this, resourceRecord);
			return resourceRecord;
        }
        
        public void ToRecord(ResourceRecord record)
        {
	        ObjectCopy.Copy(this, record);
        }
        
        public static void ToRecord(Resource res, ResourceRecord rec)
        {
	    	ObjectCopy.Copy(res, rec);
        }
        
        public void ToDB(ResourceRecord rec)
        {
			ObjectCopy.Copy(this, rec);
			if(rec.Select() == null) rec.Insert();
			else rec.Update();
        }
    }
}
