using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace SpaceInvasionBot.DAL
{
	///
	/// ActiveRecord pattern
	///
	class TableRecord
	{
		public int Id = 0;
		
		protected string tableName = typeof(TableRecord).Name;
		
		protected static string TypeInt = "INTEGER";
		protected static string TypeString = "CHARACTER";
		protected static string TypeDate = "DATETIME";
		protected Dictionary<string, string> Types = new Dictionary<string, string>()
		{
			{"Int32", TypeInt},
			{"String", TypeString},
			{"DateTime", TypeDate},
			{"System.Int32", TypeInt},
			{"System.String", TypeString},
			{"System.DateTime", TypeDate},
			{"int", TypeInt},
			{"integer", TypeInt},
			{"string", TypeString},
			{"date", TypeDate},
			{"datetime", TypeDate}
		};
		
		public TableRecord()
		{
			tableName = this.GetType().Name;
		}
		
        protected int GetId(object obj)
        {
	        foreach(FieldInfo info in obj.GetType().GetFields())
	        {
		        string name = info.Name;
		        object valu = info.GetValue(obj);
		        if(name == "Id")
					return (int)valu;
			}
			return -1;
        }
        
        public void PrintFields()
        {
			FieldInfo[] fields = this.GetType().GetFields();
			foreach (FieldInfo field in fields)
			{
			    Console.WriteLine("{2} {0}={1}", field.Name, field.GetValue(this), field.FieldType);
			}
			Console.WriteLine(BuildCreate(tableName));
			Console.WriteLine(BuildInsert(tableName));
			Console.WriteLine(BuildDelete(tableName));
			Console.WriteLine(BuildUpdate(tableName));
			Console.WriteLine(BuildSelect(tableName));
        }
        
        protected string BuildDrop(string table)
        {
	        return "DROP TABLE " + table;
        }
        
        protected string BuildCreate(string table)
        {
			string fieldfull = ", {0} {1} NOT NULL";
	        string create = "CREATE TABLE {0} (Id INT PRIMARY KEY{1});";
	        string fields = "";
	        foreach(FieldInfo info in this.GetType().GetFields())
	        {
		        string name = info.Name;
		        string type = info.FieldType.ToString();
		        
		        if(name == "Id")
		        	continue;
		        string field = string.Format(fieldfull, name, Types[type]);
		        fields += field;
	        }
	        return string.Format(create, table, fields);
        }
        
        protected string BuildInsert(string table)
        {
	        string insert = "INSERT INTO {0} ({1}) VALUES ({2});";
	        string cols = "Id";
	        string vals = "" + GetId(this);
	        foreach(FieldInfo info in this.GetType().GetFields())
	        {
		        string name = info.Name;
		        object valu = info.GetValue(this);
		        if(name == "Id")
		        	continue;
		        cols += string.Format(", {0}", name);
			    vals += string.Format(", '{0}'", valu);
	        }
	        return string.Format(insert, table, cols, vals);
        }
        
        protected string BuildDelete(string table)
        {
	        string delete = "DELETE FROM {0} WHERE Id = {1}";
	        return string.Format(delete, table, GetId(this));
        }
        
        protected string BuildUpdate(string table)
        {
	        string update = "UPDATE {0} SET {1} WHERE Id = {2}";
	        string cols = "";
	        object vali = null;
	        FieldInfo[] fields = this.GetType().GetFields();
	        bool first = true;
	        for(int i = 0;i < fields.Length;i++)
	        {
	        	FieldInfo info = fields[i];
		        string name = info.Name;
		        object valu = info.GetValue(this);
		        if(name == "Id")
		        {
			        vali = valu;
		        	continue;
	        	}
	        	if(first)
	        		cols += string.Format("{0} = '{1}'", name, valu);
	        	else
		        	cols += string.Format(", {0} = '{1}'", name, valu);
		        first = false;
	        }
	        return string.Format(update, table, cols, vali);
        }
        
        protected string BuildSelect(string table)
        {
	        string select = "SELECT * FROM {0} WHERE Id = {1}";
	        return string.Format(select, table, GetId(this));
        }
        
        protected string BuildSearch(string table)
        {
	        string select = "SELECT * FROM {0} WHERE {1}";
	        string cols = "";
	        FieldInfo[] fields = this.GetType().GetFields();
	        bool first = true;
	        for(int i = 0;i < fields.Length;i++)
	        {
	        	FieldInfo info = fields[i];
		        string name = info.Name;
		        object valu = info.GetValue(this);
		        
		        if(name == "Id")
		        	continue;
		        string type = info.FieldType.ToString();
		        string c = "'";
		        if(type == "System.Int32")
		        {
			        c = "";
		        	if((int)valu < 0)
		        		continue;
	        	}
		       	if(type == "System.String")
		       		if(valu == null)
		       			continue;
	        	if(first)
	        		cols += string.Format("{0} = {2}{1}{3}", name, valu, c ,c);
	        	else
		        	cols += string.Format(" OR {0} = {2}{1}{3}", name, valu, c, c);
		        first = false;
	        }
	        return string.Format(select, table, cols);
        }
        
        public bool Drop()
        {
	        OleDbCommand cmd = null;
            OleDbConnection con = new OleDbConnection(DB.ConnectionString);
            string sql = BuildDrop(this.tableName);
            Console.WriteLine(sql);
            bool ok = false;
            try
            {
	            cmd = new OleDbCommand(sql, con);
	            con.Open();
	            cmd.ExecuteNonQuery();
	            ok = true;
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            return ok;
        }
        
        public bool Create()
        {
	        OleDbCommand cmd = null;
            OleDbConnection con = new OleDbConnection(DB.ConnectionString);
            string sql = BuildCreate(this.tableName);
            Console.WriteLine(sql);
            bool ok = false;
            try
            {
	            cmd = new OleDbCommand(sql, con);
	            con.Open();
	            cmd.ExecuteNonQuery();
	            ok = true;
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            return ok;
        }
        
        public bool Insert()
        {
	        OleDbCommand cmd = null;
            OleDbConnection con = new OleDbConnection(DB.ConnectionString);
            string sql = BuildInsert(this.tableName);
            Console.WriteLine(sql);
            bool ok = false;
            try
            {
	            cmd = new OleDbCommand(sql, con);
	            con.Open();
	            cmd.ExecuteNonQuery();
	            ok = true;
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            return ok;
        }
        
        public bool Update()
        {
	        OleDbCommand cmd = null;
            OleDbConnection con = new OleDbConnection(DB.ConnectionString);
            string sql = BuildUpdate(this.tableName);
            Console.WriteLine(sql);
            bool ok = false;
            try
            {
	            cmd = new OleDbCommand(sql, con);
	            con.Open();
	            cmd.ExecuteNonQuery();
	            ok = true;
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            return ok;
        }
        
        public bool Delete()
        {
	        OleDbCommand cmd = null;
            OleDbConnection con = new OleDbConnection(DB.ConnectionString);
            string sql = BuildDelete(this.tableName);
            Console.WriteLine(sql);
            bool ok = false;
            try
            {
	            cmd = new OleDbCommand(sql, con);
	            con.Open();
	            cmd.ExecuteNonQuery();
	            ok = true;
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            return ok;
        }
        
        public object Select()
        {
	        OleDbConnection con = new OleDbConnection(DB.ConnectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            bool found = false;
            try
            {
                con.Open();
                cmd = new OleDbCommand(BuildSelect(this.tableName), con);
                
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if(reader.HasRows)
                {
	                found = true;
	                reader.Read();
	                foreach(FieldInfo field in this.GetType().GetFields())
	                {
		                string name = field.Name;
		                object valu = reader[name];
		                this.GetType().InvokeMember(name, BindingFlags.SetField, null, this, new object[] { valu });
	                }
                }
                reader.Close();
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                con.Close();
            }
            return found ? this : null;
        }
        
        public List<TableRecord> Search()
        {
	        OleDbConnection con = new OleDbConnection(DB.ConnectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            string sql = BuildSearch(this.tableName);
            Console.WriteLine(sql);
            
            List<TableRecord> records = new List<TableRecord>();
            try
            {
                con.Open();
                cmd = new OleDbCommand(sql, con);
                
                reader = cmd.ExecuteReader(CommandBehavior.Default);
                
                Type type = this.GetType();
                while(reader.Read())
                {
	                TableRecord record = (TableRecord)Activator.CreateInstance(type);
	                foreach(FieldInfo field in type.GetFields())
	                {
		                string name = field.Name;
		                object valu = reader[name];
		                
		                type.InvokeMember(name, BindingFlags.SetField, null, record, new object[] { valu });
	                }
	                records.Add(record);
                }
                reader.Close();
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                con.Close();
            }
            return records;
        }
        
        public List<TableRecord> SelectAll()
        {
	        OleDbConnection con = new OleDbConnection(DB.ConnectionString);
            OleDbCommand cmd = null;
            OleDbDataReader reader = null;

            string sql = "SELECT * FROM " + tableName;
            Console.WriteLine(sql);
            
            List<TableRecord> records = new List<TableRecord>();
            try
            {
                con.Open();
                cmd = new OleDbCommand(sql, con);
                
                reader = cmd.ExecuteReader(CommandBehavior.Default);
                
                Type type = this.GetType();
                while(reader.Read())
                {
	                TableRecord record = (TableRecord)Activator.CreateInstance(type);
	                foreach(FieldInfo field in type.GetFields())
	                {
		                string name = field.Name;
		                object valu = reader[name];
		                
		                type.InvokeMember(name, BindingFlags.SetField, null, record, new object[] { valu });
	                }
	                records.Add(record);
                }
                reader.Close();
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                con.Close();
            }
            return records;
        }
	}
}