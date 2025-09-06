using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PusulaAssessment
{
    public static class LongestVowelSubsequenceTask
    {
        private static readonly HashSet<char> Vowels = new HashSet<char>("aeiouAEIOU");

        /// <summary>
        /// Her kelime için ardışık sesli harflerden (aeiou) oluşan en uzun alt diziyi bulur.
        /// Sonucu [{"word":..., "sequence":..., "length":...}, ...] biçiminde JSON olarak döndürür.
        /// Eşit uzunluk durumunda kelime içinde ilk görülen dizi seçilir.
        /// </summary>
        public static string LongestVowelSubsequenceAsJson(List<string> words)
        {
            if (words == null || words.Count == 0)
                return JsonSerializer.Serialize(new List<object>());

            var list = new List<object>(words.Count);

            foreach (var word in words)
            {
                if (string.IsNullOrEmpty(word))
                {
                    list.Add(new { word = word ?? "", sequence = "", length = 0 });
                    continue;
                }

                int bestStart = -1, bestLen = 0;
                int curStart = -1, curLen = 0;

                for (int i = 0; i < word.Length; i++)
                {
                    if (Vowels.Contains(word[i]))
                    {
                        if (curStart == -1) curStart = i;
                        curLen++;
                    }
                    else
                    {
                        // close current run
                        if (curLen > bestLen)
                        {
                            bestLen = curLen;
                            bestStart = curStart;
                        }
                        curStart = -1;
                        curLen = 0;
                    }
                }

                // finalize last run if word ends with vowels
                if (curLen > bestLen)
                {
                    bestLen = curLen;
                    bestStart = curStart;
                }

                string seq = bestLen > 0 && bestStart >= 0 ? word.Substring(bestStart, bestLen) : "";
                list.Add(new { word, sequence = seq, length = bestLen });
            }

            return JsonSerializer.Serialize(list);
        }
    }
}
