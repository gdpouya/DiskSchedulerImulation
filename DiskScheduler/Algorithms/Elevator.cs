using DiskSchedulerImulation.Classes;
namespace DiskSchedulerImulation.Algorithms
{
    public class Elevator
    {
        public static (List<int>, List<double>) GetElevator(int headPosition, List<int> requestedCylinder,
                                                      List<int> requestTimes, double rotationalLatency,
                                                      double transferTime)
        {
            List<int> respondedCylinder = new List<int>();
            List<double> responseTimes = new List<double>();
            double elapsedTime = 0.0;
            int endIndex = 0;
            int move = -1;
            double trackReadTime = rotationalLatency + transferTime;

            while (requestedCylinder.Any())
            {
                endIndex = Utility.RequestReceived(requestTimes, elapsedTime);

                if (endIndex == -1)
                {
                    elapsedTime = requestTimes[0];
                    continue;
                }

                (int responseIndex, int newMove) = SelectRequest(move, endIndex, requestedCylinder, headPosition);

                if (responseIndex == -1)
                {
                    continue;
                }

                List<int> midWayCylinders = MidwayRequests(headPosition, responseIndex, requestedCylinder,
                                                            requestTimes, elapsedTime, move);

                (respondedCylinder, responseTimes, elapsedTime, requestedCylinder, requestTimes, headPosition,
                 responseIndex, endIndex) =
                    ResponseMidwayRequested(headPosition, requestedCylinder, requestTimes, respondedCylinder,
                                            responseTimes, elapsedTime, midWayCylinders, responseIndex,
                                            rotationalLatency, transferTime, endIndex);

                double headTime = Utility.CylinderDistance(requestedCylinder[responseIndex], headPosition);
                if (move != -1)
                {
                    elapsedTime += headTime + Variabels.HEAD_DELAY + trackReadTime;
                }
                else
                {
                    elapsedTime += trackReadTime;
                }

                responseTimes.Add(elapsedTime);
                respondedCylinder.Add(requestedCylinder[responseIndex]);
                headPosition = requestedCylinder[responseIndex];

                requestTimes.RemoveAt(responseIndex);
                requestedCylinder.RemoveAt(responseIndex);

                if (responseIndex <= endIndex)
                {
                    endIndex -= 1;
                }
            }

            return (respondedCylinder, responseTimes);
        }

        public static (int, int) SelectRequest(int move, int endIndex, List<int> requestedCylinder, int headPosition)
        {
            if (move == -1)
            {
                if (requestedCylinder[0] > headPosition)
                {
                    return (0, 1);
                }
                else if (requestedCylinder[0] < headPosition)
                {
                    return (0, 0);
                }
                else
                {
                    return (0, -1);
                }
            }
            else if (move == 1)
            {
                for (int i = 0; i <= endIndex; i++)
                {
                    if (requestedCylinder[i] >= headPosition)
                    {
                        return (i, move);
                    }
                }
                return (-1, 0);
            }
            else
            {
                for (int i = 0; i <= endIndex; i++)
                {
                    if (requestedCylinder[i] <= headPosition)
                    {
                        return (i, move);
                    }
                }
                return (-1, 1);
            }
        }
        public static List<int> MidwayRequests(int headPosition, int responseIndex, List<int> requestedCylinder,
                                     List<int> requestTimes, double elapsedTime, int move)
        {
            List<int> cylinders = new List<int>();

            for (int i = 0; i < requestTimes.Count; i++)
            {
                int elapsed_time_to_mid_request = Math.Abs(requestedCylinder[i] - headPosition) / Variabels.TRACK_TRAVERSED_IN_ONE_MS;
                if (move == 1)
                {
                    if (headPosition < requestedCylinder[i] && requestedCylinder[i] < requestedCylinder[responseIndex]
                        && elapsed_time_to_mid_request + elapsedTime >= requestTimes[i])
                    {
                        cylinders.Insert(0, requestedCylinder[i]);
                    }
                }
                else if (move == 0)
                {
                    if (headPosition > requestedCylinder[i] && requestedCylinder[i] > requestedCylinder[responseIndex]
                        && elapsed_time_to_mid_request + elapsedTime >= requestTimes[i])
                    {
                        cylinders.Insert(0, requestedCylinder[i]);
                    }
                }
            }

            if (cylinders.Count > 0)
            {
                if (move == 1)
                {
                    cylinders.Sort();
                }
                else
                {
                    cylinders.Sort((a, b) => b.CompareTo(a));
                }
            }

            return cylinders;
        }
        public static (List<int>, List<double>, double, List<int>, List<int>, int, int, int) ResponseMidwayRequested(int headPosition,
                                            List<int> requestedCylinder, List<int> requestTimes,
                                            List<int> respondedCylinder, List<double> responseTimes,
                                            double elapsedTime, List<int> midwayCylinders, int responseIndex,
                                            double rotationalLatency, double transferTime, int endIndex)
        {
            double trackReadTime = rotationalLatency + transferTime;

            foreach (int cylinder in midwayCylinders)
            {
                double headTime = Utility.CylinderDistance(cylinder, headPosition);
                elapsedTime += headTime + Variabels.HEAD_DELAY + trackReadTime;

                respondedCylinder.Add(cylinder);
                responseTimes.Add(elapsedTime);
                headPosition = cylinder;

                int index = requestedCylinder.IndexOf(cylinder);
                requestTimes.RemoveAt(index);
                requestedCylinder.RemoveAt(index);

                if (index < responseIndex)
                {
                    responseIndex -= 1;
                }
                if (index <= endIndex)
                {
                    endIndex -= 1;
                }
            }

            return (respondedCylinder, responseTimes, elapsedTime, requestedCylinder, requestTimes, headPosition, responseIndex, endIndex);
        }
    }
}