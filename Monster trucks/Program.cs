using Monster_trucks.Data;
using Monster_trucks.Services;

namespace Monster_trucks
{
    class Program
    {
        static void Main()
        {
            var dbConnection = new DatabaseConnection();
            var facade = new MonsterTrackerFacade(dbConnection);

            var ui = new UI.ConsoleUI(facade);
            ui.Run();
        }
    }
}
