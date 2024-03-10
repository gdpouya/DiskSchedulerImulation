using System;
using System.Collections.Generic;
using System.Linq;
using DiskSchedulerImulation.Classes;
public class DiskScheduler
{
    

    public static void Main(string[] args)
    {
        while (true)
        {
            Messages.WellcomeMassages();
            int userInput = Convert.ToInt32(Console.ReadLine());
            switch (userInput)
            {
                case 1:
                    DataGenarator.ManualInput();
                    break;
                case 2:
                    DataGenarator.RandomGenerator();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Error : Please select an option between 1 to 3!");
                    break;
            }
        }
    }

   

    
    
    
}

