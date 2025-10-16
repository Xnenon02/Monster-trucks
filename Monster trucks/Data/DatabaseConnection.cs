using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Monster_trucks.Data
{
    public static class DatabaseConnection
    {
        private const string ConnectionString = "Data Source=monstertracker.db";

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }
    }
}
