using DiskSchedulerImulation.Classes;
namespace DiskSchedulerImulation.Algorithms
{
    public class FCFS
    {
        public static (List<int>, List<double>) GetFCFS(int headPosition, List<int> requestedCylinder,
                                                          List<int> requestTimes, double rotationalLatency,
                                                          double transferTime)
        {
            List<int> respondedCylinder = new List<int>();
            List<double> responseTimes = new List<double>();
            double elapsedTime = 0.0;
            double distance = 0.0;
            double trackReadTime = rotationalLatency + transferTime;

            for (int i = 0; i < requestedCylinder.Count; i++)
            {
                if (elapsedTime < requestTimes[i])
                {
                    elapsedTime += requestTimes[i] - elapsedTime;
                }

                distance = Utility.CylinderDistance(requestedCylinder[i], headPosition);

                if (distance > 0)
                {
                    elapsedTime += distance + Variabels.HEAD_DELAY + trackReadTime;
                }
                else
                {
                    elapsedTime += trackReadTime;
                }

                responseTimes.Add(elapsedTime);
                respondedCylinder.Add(requestedCylinder[i]);
                headPosition = requestedCylinder[i];
            }

            return (respondedCylinder, responseTimes);
        }
    }
}