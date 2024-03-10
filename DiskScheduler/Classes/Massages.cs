using System;
using System.Collections.Generic;

namespace DiskSchedulerImulation.Classes
{
    public class Messages
    {
        public static void WellcomeMassages()
        {
            Console.WriteLine("How would you like to work with the simulator:");
            Console.WriteLine("1- Enter My own Data.");
            Console.WriteLine("2- Random Data generator.");
            Console.WriteLine("3- Exit.");
        }

        public static void PrintSolution(List<int> elevatorCylinderResponded, List<double> elevatorTimeResponded,
                                          List<int> fcfsCylinderResponded, List<double> fcfsTimeResponded)
        {
            Console.WriteLine($"              Elevator                                        FCFS");
            Console.WriteLine("Cylinder of request     Time request           Cylinder of request    Time request");

            for (int i = 0; i < elevatorCylinderResponded.Count; i++)
            {
                Console.WriteLine($"{elevatorCylinderResponded[i],-24} {elevatorTimeResponded[i],-22:F1}" +
                                  $"{fcfsCylinderResponded[i],-28} {fcfsTimeResponded[i],-22:F1}");
            }
        }
    }
}
