using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part1
{
    static class TaxCalculator
    {
        // Create a static dictionary field that holds a List of TaxRecords and is keyed by a string
        private static Dictionary<string, List<TaxRecord>> record = new Dictionary<string,List<TaxRecord>>();

        // create a static constructor that:
        static TaxCalculator()
        {
            // declare a streamreader to read a file
            // enter a try/catch block for the entire static constructor to print out a message if an error occurs
            // initialize the static dictionary to a newly create empty one
            // open the taxtable.csv file into the streamreader
            string filename = @"taxtable.csv";
            try
            { using (StreamReader read = new StreamReader(filename))

                {   // loop over the lines from the streamreader
                    // read a line from the file


                    foreach (string line in File.ReadLines(filename))
                    {
                        try
                        {
                            List<TaxRecord> records;
                            // constuct a taxrecord from the (csv) line in the file
                            TaxRecord tax = new TaxRecord(line);
                            // see if the state in the taxrecord is already in the dictionary
                            bool ispresent = record.TryGetValue(tax.Statecode, out records);
                            //     if it is:  add the new tax record to the list of records in that state
                            if (ispresent) { records.Add(tax); }

                            //     if it is not
                            else
                            {
                                // create a new list of taxrecords
                                records = new List<TaxRecord>();
                                //            add the new taxrecord to the list
                                records.Add(tax);

                                //            add the list to the dictionary under the state for the taxrecord
                                record.Add(tax.Statecode, records);

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        //provide a way to get out of the loop when you are done with the file....
                        // catch any exceptions while processing each line in another try/catch block located INSIDE the loop
                        //   this way if the line in the CSV file is incorrect, you will continue to process the next line
                        // make sure the streamreader is disposed no matter what happens

                    }
                }
            
            }catch (Exception ex)
            
            { Console.WriteLine(ex);

            }    

        }
        // create a static method (ComputeTaxFor)  to return the computed tax given a state and income
        //  use the state as a key to find the list of taxrecords for that state
        //   throw an exception if the state is not found.
        //   otherwise use the list to compute the taxes

        public static decimal ComputeTaxFor(string code, decimal income)
        {
            decimal finaltax = 0M;
            //  Create a variable to hold the final computed tax.  set it to 0
            //  loop over the list of taxrecords for the state
            List<TaxRecord> codename;
            decimal totaltax = 0M;
            try { if (record.TryGetValue(code.ToUpper(), out codename))
                { 
                    foreach (var records in codename)
                    { if (income > records.highIncome )
                        {
                             totaltax = (records.highIncome - records.lowIncome) * records.Rate;
                             finaltax += totaltax;
                        }

                        if (income < records.highIncome && income > records.lowIncome)
                        {
                            totaltax = (income - records.lowIncome) * records.Rate;
                            finaltax += totaltax;
                            return finaltax;
                        }
                        
                    }
                        
                            } 
            } catch (Exception)
            {  Console.WriteLine($"Invalid statecode:{code}"); }

            return finaltax;
        }
    }  // this is the end of the Tax Calculator
       

  


    // try to figure out how to implement the Verbose option AFTER you have everything else done.


    class TaxRecord
    {
        // create a TaxRecord class representing a line from the file.  It shoudl have public properties of the correct type
        // for each of the columns in the file
        //  StateCode   (used as the key to the dictionary)
        //  State       (Full state name)
        //  Floor       (lowest income for this tax bracket)
        //  Ceiling     (highest income for this tax bracket )
        //  Rate        (Rate at which income is taxed for this tax bracket)
        //
        public string Statecode { get; set; }
        public string Nameofstate { get; set; }
        public decimal lowIncome { get; set; }
        public decimal highIncome { get; set; }

        public decimal Rate { get; set; }
   

        public TaxRecord(string csv)
        {

            //  Create a ctor taking a single string (a csv) and use it to load the record
            //  Be sure to throw detailed exceptions when the data is invalid
            //

            const char Separator = ',';

            string[] data = csv.Split(Separator);
            if (5 != data.Length)
            {
                throw new Exception($"CSV Entry {csv}does not read five values(state ID," +
                    $" state,minimum income, maxium income, rate)");
            }

            Statecode = data[0];
            Nameofstate = data[1];
            decimal temp1;

            if (!decimal.TryParse(data[2], out temp1)) { throw new Exception($"Entry {data[2]} is not a decimal"); }
            lowIncome = temp1;

            if (!decimal.TryParse(data[3], out temp1)) { throw new Exception($"Entry {data[3]} is not a decimal"); }
            highIncome = temp1;

            if (!decimal.TryParse(data[4], out temp1)) { throw new Exception($"Entry {data[4]} is not a decimal"); }
            Rate = temp1;
        } //this is the end of taxrecord
        //  Create an override of ToString to print out the tax record info nicely
        public override string ToString()
        { 

            return $"State ID:{Statecode} \tState:\t{Nameofstate} \tRate:\t{Rate} \tLowincome:\t {lowIncome,15}" +
                $"\tHghincome\t{highIncome,15}";
        }
    
       


    }  // this is the end of the TaxRecord

    class Program
    {
        public static void Main()
        {
    
            // create an infinite loop to:

            
            // validate the data


            
            do

            {
                decimal tax = 0;
                string temp;
                decimal income = 0;
                string state;
                // prompt the user for a state and an income
                Console.WriteLine($"Please enter the state:");
                state = Console.ReadLine();
                Console.WriteLine($"Please enter an income:");
                 temp= Console.ReadLine();
                try
                {
                    income = decimal.Parse(temp);

                }
                catch (Exception)
                {
                    Console.WriteLine($"Invalid inputs for a decimal income:{temp}");
                }
                // calculate the tax due and print out the total

                tax = TaxCalculator.ComputeTaxFor(state, income);
                Console.WriteLine(tax);
                        // loop
            } while (true);


        }
    }

}
