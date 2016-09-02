using System;
using SpaceInvasionBot.DAL;

namespace SpaceInvasionBot
{
	class ObjectDumper
	{
		public static void Dump(object obj, TableRecord rec)
		{
			if(obj == null)
				return ;
			ObjectCopy.Copy(obj, rec);
			if(rec.Select() == null) rec.Insert();
			else rec.Update();
		}
		
		public static void Dump(TableRecord rec)
		{
			if(rec.Select() == null) rec.Insert();
			else rec.Update();
		}

	}
}