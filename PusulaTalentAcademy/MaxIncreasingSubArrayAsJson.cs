using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PusulaAssessment
{
    public static class MaxIncreasingSubArrayTask
    {
        /// <summary>
        /// Verilen listede ardışık olarak artan (strictly increasing) tüm alt diziler arasında
        /// TOPLAMI en yüksek olan alt diziyi bulur ve JSON (array) olarak döndürür.
        /// Boş giriş için "[]" döner.
        /// Eşit toplam durumunda daha uzun olan; yine eşitlikte dizide daha erken başlayan tercih edilir.
        /// </summary>
        public static string MaxIncreasingSubArrayAsJson(List<int> numbers)
        {
            if (numbers == null || numbers.Count == 0)
                return JsonSerializer.Serialize(new List<int>());

            int bestStart = 0, bestEnd = 0; // inclusive indices
            long bestSum = numbers[0];

            int curStart = 0;
            long curSum = numbers[0];

            for (int i = 1; i < numbers.Count; i++)
            {
                if (numbers[i] > numbers[i - 1])
                {
                    // Run continues
                    curSum += numbers[i];
                }
                else
                {
                    // Run ends at i-1, evaluate
                    UpdateBest(numbers, curStart, i - 1, curSum, ref bestStart, ref bestEnd, ref bestSum);

                    // start new run at i
                    curStart = i;
                    curSum = numbers[i];
                }
            }

            // finalize last run
            UpdateBest(numbers, curStart, numbers.Count - 1, curSum, ref bestStart, ref bestEnd, ref bestSum);

            var result = numbers.GetRange(bestStart, bestEnd - bestStart + 1);
            return JsonSerializer.Serialize(result);
        }

        private static void UpdateBest(List<int> nums, int s, int e, long sum,
            ref int bestS, ref int bestE, ref long bestSum)
        {
            if (sum > bestSum)
            {
                bestSum = sum;
                bestS = s;
                bestE = e;
                return;
            }

            if (sum == bestSum)
            {
                int curLen = e - s + 1;
                int bestLen = bestE - bestS + 1;
                if (curLen > bestLen || (curLen == bestLen && s < bestS))
                {
                    bestS = s;
                    bestE = e;
                }
            }
        }
    }
}
