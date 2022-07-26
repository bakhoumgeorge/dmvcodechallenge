using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DmvAppointmentScheduler
{
    class Program
    {
        public static Random random = new Random();
        public static List<Appointment> appointmentList = new List<Appointment>();
        static void Main(string[] args)
        {
            CustomerList customers = ReadCustomerData();
            TellerList tellers = ReadTellerData();
            Calculation(customers, tellers);
            OutputTotalLengthToConsole();

        }
        private static CustomerList ReadCustomerData()
        {
            string fileName = "CustomerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData\", fileName);
            string jsonString = File.ReadAllText(path);
            CustomerList customerData = JsonConvert.DeserializeObject<CustomerList>(jsonString);
            return customerData;

        }
        private static TellerList ReadTellerData()
        {
            string fileName = "TellerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData\", fileName);
            string jsonString = File.ReadAllText(path);
            TellerList tellerData = JsonConvert.DeserializeObject<TellerList>(jsonString);
            return tellerData;

        }
        static void Calculation(CustomerList customers, TellerList tellers)
        {
            List<Teller> zeroType = new List<Teller>();
            List<Teller> oneType = new List<Teller>();
            List<Teller> twoType = new List<Teller>();
            List<Teller> threeType = new List<Teller>();

            foreach (Teller teller in tellers.Teller)
            {
                switch (teller.specialtyType)
                {
                    case "0":
                        zeroType.Add(teller);
                        break;
                    case "1":
                        oneType.Add(teller);
                        break;
                    case "2":
                        twoType.Add(teller);
                        break;
                    case "3":
                        threeType.Add(teller);
                        break;
                }
            }

            int zero = 0, one = 0, two = 0, three = 0;

            foreach (Customer customer in customers.Customer)
            {
                switch (customer.type)
                {
                    case "1":
                        var appointment1 = new Appointment(customer, tellers.Teller[one++]);
                        appointmentList.Add(appointment1);

                        if (one >= oneType.Count) one = 0;
                        break;
                    case "2":
                        var appointment2 = new Appointment(customer, tellers.Teller[two++]);
                        appointmentList.Add(appointment2);

                        if (two >= twoType.Count) two = 0;
                        break;
                    case "3":
                        var appointment3 = new Appointment(customer, tellers.Teller[three++]);
                        appointmentList.Add(appointment3);

                        if (three >= threeType.Count) three = 0;
                        break;
                    case "4":
                        var appointment0 = new Appointment(customer, tellers.Teller[zero++]);
                        appointmentList.Add(appointment0);

                        if (zero >= zeroType.Count) zero = 0;
                        break;
                }
            }
        }
        static void OutputTotalLengthToConsole()
        {
            var tellerAppointments =
                from appointment in appointmentList
                group appointment by appointment.teller into tellerGroup
                select new
                {
                    teller = tellerGroup.Key,
                    totalDuration = tellerGroup.Sum(x => x.duration),
                };
            var max = tellerAppointments.OrderBy(i => i.totalDuration).LastOrDefault();
            Console.WriteLine("Teller " + max.teller.id + " will work for " + max.totalDuration + " minutes!");
        }

    }
}
