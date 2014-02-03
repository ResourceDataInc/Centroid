using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentroidConfig;

namespace CentroidConfig.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Centroid();
            foreach (string env in new [] { "Dev", "Test", "Prod" })
            {
                dynamic config = c.Environment(env);
	            var admin_user = config.Database.admin.user_name;
	            var admin_password = config.Database.admin.password;
	            var db = config.Database.Server;

                Console.WriteLine(String.Format("{0} => Server={1};Username={2};Password={3}", env, db, admin_user, admin_password));
            }

            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
