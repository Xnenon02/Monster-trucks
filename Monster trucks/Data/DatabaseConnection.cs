using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Monster_trucks.Data
{
    public class DatabaseConnection
    {
        private const string ConnectionString = "Data Source=monstertracker.db;Version=3;";

        public SqliteConnection GetConnection()
        {
            var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }

}
//git test nothing to see