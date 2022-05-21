using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2
{


    class EmployeeRecord
    {
        // create an employee Record with public properties
        //    ID                        a number to identify an employee
        public int Employee_ID { get; set; }
        //    Name                      the employee name
        public string Employee_Name { get; set; }
        //    StateCode                 the state collecting taxes for this employee 
        public string Statecode { get; set; }
        //    HoursWorkedInTheYear      the total number of hours worked in the entire year (including fractions of an hour)
        public decimal HrsWorkedInTheYear { get; set; }
        //    HourlyRate                the rate the employee is paid for each hour worked
        //                                  assume no changes to the rate throughout the year.
        public decimal HourlyRate { get; set; }

        //    provide a constructor that takes a csv and initializes the employeerecord
        //       do all error checking and throw appropriate exceptions
        public EmployeeRecord(string csv)
        {
            const char Separator = ',';

            string[] data = csv.Split(Separator);
            if (5 != data.Length)
            {
                throw new Exception($"Not enough arguments for constructor");
            }

            Employee_ID = int.Parse(data[0]);
            Employee_Name = data[1];
            if (data.Length == 2) { throw new Exception($"StateCode consists of more than 2 characters."); }
            Statecode = data[2];

            decimal temp;
            if (!decimal.TryParse(data[3], out temp)) { throw new Exception($"Entry {data[3]} is not a deciaml"); }
            HrsWorkedInTheYear = temp;

            if (!decimal.TryParse(data[4], out temp)) { throw new Exception($"Entry {data[4]} is not a decimal"); }
            HourlyRate = temp;


        }

        //    provide an additional READ ONLY property called  YearlyPay that will compute the total income for the employee
        //        by multiplying their hours worked by their hourly rate
        public decimal YearlyPay { get { return this.HrsWorkedInTheYear * this.HourlyRate; } }

        //    provide an additional READONLY property that will compute the total tax due by:
        // calling into the taxcalculator providing the statecode and the yearly income computed in the YearlyPay property
        public decimal Taxtotal { get { return Part1.TaxCalculator.ComputeTaxFor(Statecode, YearlyPay); } }

        //JC note: For some unknown reason I cannot get an override  Tostring() and it is unable to print anything
        //    provide an override of toString to output the record : including the YearlyPay and the TaxDue
        public override string ToString()
        {
            return $"Empolyee ID:\t{Employee_ID} \tEmployee Name:\t {Employee_Name} \tState code:\t {Statecode}" +
                   $"\tYearly Pay:\t {YearlyPay,15} \tTax Due: {Taxtotal,15}";
        }
    }

    static class EmployeesList
    {
        // create an EmployeeList class that will read all the employees form the Employees.csv file
        // the logic is similar to the way the taxcalculator read its taxrecords
        public static List<EmployeeRecord> Employee = new List<EmployeeRecord>();
        static EmployeesList()
        {
            // declare a streamreader to read a file
            // enter a try/catch block for the entire static constructor to print out a message if an error occurs
            // initialize the static dictionary to a newly create empty one
            // open the taxtable.csv file into the streamreader
            string filename1 = @"employees.csv";
            try
            {
                using (StreamReader read = new StreamReader(filename1))

                {   // loop over the lines from the streamreader
                    // read a line from the file


                    foreach (string line in File.ReadLines(filename1))
                    {
                        try
                        {
                            // Create a List of employee records.  The employees are arranged into a LIST not a DICTIONARY
                            //   because we are not accessing the employees by state,  we are accessing the employees sequentially as a list

                            // create a static constructor to load the list from the file
                            //   be sure to include try/catch to display messages
                            EmployeeRecord emp = new EmployeeRecord(line);
                            Employee.Add(emp);

                           
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    read.Close();

                }
            }
            catch (Exception ex)
            { Console.WriteLine($"ERORR:{ex.Message}"); 
            }
        }
    }



    class Program
    {

        // loop over all the employees in the EmployeeList and print them
        // If the ToString() in the employee record is correct, all the data will print out.

        public static void Main()
        {
            
            try
            { // write your logic here
                foreach (EmployeeRecord emp in EmployeesList.Employee) {

                    Console.WriteLine($"Employee ID:{emp.Employee_ID} \t Employee Name: {emp.Employee_Name}" +
                        $"\nState ID:{emp.Statecode} \t Yearly Pay{emp.YearlyPay:0.00}\nTax Due {emp.Taxtotal:0.00}" +
                        $"\t Hourly Rate{emp.HourlyRate:0.00}\n Yearly hours worked:{emp.HrsWorkedInTheYear:0.00}" );
       



                }

            }



            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}

