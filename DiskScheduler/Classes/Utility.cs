namespace DiskSchedulerImulation.Classes
{
    public class Utility
    {
        public static int RequestReceived(List<int> requestTime, double elapsedTime)
        {
            int right = requestTime.Count - 1;
            int result = -1;
            int left = 0;

            while (left <= right)
            {
                int mid = (left + right) / 2;

                if (mid == requestTime.Count - 1)
                {
                    result = mid;
                }

                if (requestTime[mid] > elapsedTime)
                {
                    result = mid - 1;
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }

            return result;
        }

        public static (List<int>, List<int>) SortRequests(List<int> requestedCylinder, List<int> requestTimes, int move = 1)
        {
            List<(int, int)> combinedLists = requestedCylinder.Zip(requestTimes, (c, t) => (c, t)).ToList();
            List<(int, int)> sortedCombinedLists;

            if (move == 1)
            {
                sortedCombinedLists = combinedLists.OrderBy(x => x.Item2).ToList();
            }
            else
            {
                sortedCombinedLists = combinedLists.OrderByDescending(x => x.Item2).ToList();
            }

            List<int> sortedCylinder = sortedCombinedLists.Select(x => x.Item1).ToList();
            List<int> sortedTimes = sortedCombinedLists.Select(x => x.Item2).ToList();

            return (sortedCylinder, sortedTimes);
        }
        public static double CylinderDistance(int requestedCylinder, int headPosition)
        {
            return Math.Abs((requestedCylinder - headPosition) / Variabels.TRACK_TRAVERSED_IN_ONE_MS);
        }

        public static void SaveToCSV(List<int> cylinder, List<double> time, string fileName)
        {
            var rows = cylinder.Zip(time, (c, t) => $"{c},{t}");

            File.WriteAllLines(fileName, rows);
        }
    }
}