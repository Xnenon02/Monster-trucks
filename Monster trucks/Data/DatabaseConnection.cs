using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Monster_trucks.Data
{
    public static class DatabaseConnection
    {
        private const string ConnectionString = "Data Source=monstertracker.db";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }
    }

}
