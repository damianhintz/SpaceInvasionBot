using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;

namespace SpaceInvasionBot.DAL
{
    static class DB
    {
        private static string _connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Bot.mdb;";
        public static string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }
    }
}
