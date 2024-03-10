using System;
using DiskSchedulerImulation.Algorithms;
namespace DiskSchedulerImulation.Classes
{
    public class DataGenarator
    {
        public static void ManualInput()
        {
            (int headPosition, List<int> requestedCylinder, List<int> requestTimes) = Reader();
            (requestedCylinder, requestTimes) = Utility.SortRequests(requestedCylinder, requestTimes);
            double rotationalLatency = 4.17;
            double transferTime = 0.13;

            (List<int> elevatorCylinderResponded, List<double> elevatorTimeResponded) = Elevator.GetElevator(
                headPosition, requestedCylinder, requestTimes, rotationalLatency, transferTime);

            (List<int> fcfsCylinderResponded, List<double> fcfsTimeResponded) = FCFS.GetFCFS(
                headPosition, requestedCylinder, requestTimes, rotationalLatency, transferTime);


            Utility.SaveToCSV(elevatorCylinderResponded, elevatorTimeResponded, "Output/elevator_data.csv");
            Utility.SaveToCSV(fcfsCylinderResponded, fcfsTimeResponded, "Output/fcfs_data.csv");

            Messages.PrintSolution(elevatorCylinderResponded, elevatorTimeResponded,
                                    fcfsCylinderResponded, fcfsTimeResponded);
        }
        public static (int, List<int>, List<int>) Reader()
        {
            int head = Convert.ToInt32(Console.ReadLine());
            int k = Convert.ToInt32(Console.ReadLine());
            List<int> cylinder = new List<int>();
            List<int> time = new List<int>();

            for (int i = 0; i < k; i++)
            {
                int[] entry = Console.ReadLine().Split().Select(int.Parse).ToArray();
                cylinder.Add(entry[0]);
                time.Add(entry[1]);
            }

            return (head, cylinder, time);
        }
        public static void RandomGenerator()
        {
            Console.WriteLine("Please enter the number of random entries you want to generate: ");
            int userInput = int.Parse(Console.ReadLine());

            List<List<int>> requestedCylinder = new List<List<int>>();
            List<List<int>> requestedTimes = new List<List<int>>();
            List<int> headPosition = new List<int>();
            List<double> rotationalLatency = new List<double>();
            List<double> transferTime = new List<double>();

            Random rand = new Random();

            for (int i = 0; i < userInput; i++)
            {
                List<int> tempCylinder = new List<int>();
                List<int> tempTimes = new List<int>();

                for (int j = 0; j < 100; j++)
                {
                    tempCylinder.Add(rand.Next(0, 100000));
                    tempTimes.Add(rand.Next(0, 100));
                }

                requestedCylinder.Add(tempCylinder);
                requestedTimes.Add(tempTimes);
                headPosition.Add(rand.Next(0, 100000));
                rotationalLatency.Add(Math.Round(rand.NextDouble() * (8.0 - 2.0) + 2.0, 2));
                transferTime.Add(Math.Round(rand.NextDouble() * (0.20 - 0.01) + 0.01, 2));
            }

            List<double> elevatorTimes = new List<double>();
            List<double> fcfsTimes = new List<double>();
            List<double> elevatorLastTimes = new List<double>();
            List<double> fcfsLastTimes = new List<double>();
            List<double> elevatorAverage = new List<double>(new double[100]);
            List<double> fcfsAverage = new List<double>(new double[100]);

            for (int i = 0; i < userInput; i++)
            {
                var sortedRequests = Utility.SortRequests(requestedCylinder[i], requestedTimes[i]);
                List<int> requestedCylinder_i = sortedRequests.Item1;
                List<int> requestedTimes_i = sortedRequests.Item2;
                int headPosition_i = headPosition[i];
                double rotationalLatency_i = rotationalLatency[i];
                double transferTime_i = transferTime[i];

                var fcfsResponse = FCFS.GetFCFS(headPosition_i, requestedCylinder_i, requestedTimes_i, rotationalLatency_i, transferTime_i);
                (_, List<double> fcfsTimeResponded) = fcfsResponse;
                var elevatorResponse = Elevator.GetElevator(headPosition_i, requestedCylinder_i, requestedTimes_i, rotationalLatency_i, transferTime_i);
                (_, List<double> elevatorTimeResponded) = elevatorResponse;

                elevatorTimes.AddRange(elevatorTimeResponded);
                fcfsTimes.AddRange(fcfsTimeResponded);

                elevatorLastTimes.Add(elevatorTimeResponded.Last());
                fcfsLastTimes.Add(fcfsTimeResponded.Last());

                for (int j = 0; j < elevatorAverage.Count; j++)
                {
                    elevatorAverage[j] += elevatorTimeResponded[j];
                    fcfsAverage[j] += fcfsTimeResponded[j];
                }
            }
        }
    }
}