namespace RedisConsole
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using RedisManager.Class;
    using RedisManager.Entity;

    class Program
    {
        const string SecretName = "CacheConnection";
        const string TimeExpired = "TimeExpired";

        private static IConfigurationRoot ConfigRedis { get; set; }

        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            ConfigRedis = builder.Build();
        }

        static void Main(string[] args)
        {
            InitializeConfiguration();

            var conn = new Connection(ConfigRedis[SecretName]);

            Console.WriteLine("Do you want to test the connection?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            int option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
                Console.WriteLine(conn.TestConnection());

            Console.WriteLine("-------------------EMPLOYEES-------------------");

            int time = Convert.ToInt32(ConfigRedis[TimeExpired]);
            var data = new CacheManager<List<Employee>>(conn, time);

            Console.WriteLine("Select an option");
            Console.WriteLine("1. Add employee");
            Console.WriteLine("2. Show employee list");
            Console.WriteLine("3. Exit");
            Console.WriteLine("If you choose other option, the app will close");

            option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    AddData(data);
                    break;
                case 2:
                    ShowData(data);
                    break;
                case 3:
                default:
                    Environment.Exit(0);
                    break;
            }

            Console.ReadKey();
        }

        static void AddData(CacheManager<List<Employee>> cache)
        {
            Console.WriteLine("Initialize writing, please wait... \n");

            Console.WriteLine("Type the employee id");
            string id = Console.ReadLine();

            Console.WriteLine("Type the employee full name");
            string name = Console.ReadLine();

            Console.WriteLine("Type the employee age");
            int age = Convert.ToInt32(Console.ReadLine());

            var employee = new List<Employee>
            {
                new Employee
                {
                    Id = id,
                    Name = name,
                    Age = age
                }
            };

            cache.SetValue("employee", employee);

            Console.WriteLine("Data written successfully");
        }

        static void ShowData(CacheManager<List<Employee>> cache)
        {
            if (cache.Exists("employee"))
            {
                Console.WriteLine("Loading data, please wait... \n");

                var employee = cache.GetValue("employee");

                foreach (var item in employee)
                    Console.WriteLine($"Document Id: {item.Id}; Name: {item.Name}; Age: {item.Age}; \n");

                Console.WriteLine("Data loaded successfully");
            }
            else
            {
                Console.WriteLine("Data not exists or not found");
            }
        }
    }
}
