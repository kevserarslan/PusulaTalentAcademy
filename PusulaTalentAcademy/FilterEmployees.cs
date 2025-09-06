using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PusulaAssessment
{
    public static class FilterEmployeesTask
    {
        /// <summary>
        /// Statik tuple listesi üzerinde şu şartları uygular:
        /// - 25 <= Age <= 40
        /// - Department in {"IT","Finance"}
        /// - 5000m <= Salary <= 9000m
        /// - HireDate >= 2017-01-01   (örnek 5 ile uyumlu olacak şekilde)
        ///
        /// Sonra:
        /// - İsimleri uzunluk DESC, ardından alfabetik ASC sıralar
        /// - Toplam, Ortalama(2hane), Min, Max, Count hesaplar
        /// Sonucu JSON döndürür.
        /// </summary>
        public static string FilterEmployees(IEnumerable<(string Name, int Age, string Department, decimal Salary, DateTime HireDate)> employees)
        {
            if (employees == null)
            {
                var empty = new { Names = Array.Empty<string>(), TotalSalary = 0m, AverageSalary = 0m, MinSalary = 0m, MaxSalary = 0m, Count = 0 };
                return JsonSerializer.Serialize(empty);
            }

            DateTime hireLimit = new DateTime(2017, 1, 1);
            string[] allowed = new[] { "IT", "Finance" };

            var filtered = employees
                .Where(e => e.Age >= 25 && e.Age <= 40)
                .Where(e => allowed.Contains(e.Department, StringComparer.OrdinalIgnoreCase))
                .Where(e => e.Salary >= 5000m && e.Salary <= 9000m)
                .Where(e => e.HireDate >= hireLimit)
                .ToList();

            var names = filtered
                .Select(e => e.Name)
                .OrderByDescending(n => n?.Length ?? 0)
                .ThenBy(n => n)
                .ToList();

            decimal total = filtered.Sum(e => e.Salary);
            decimal min = filtered.Count > 0 ? filtered.Min(e => e.Salary) : 0m;
            decimal max = filtered.Count > 0 ? filtered.Max(e => e.Salary) : 0m;
            int count = filtered.Count;
            decimal avg = count > 0 ? Math.Round(total / count, 2, MidpointRounding.AwayFromZero) : 0m;

            var result = new
            {
                Names = names,
                TotalSalary = total,
                AverageSalary = avg,
                MinSalary = min,
                MaxSalary = max,
                Count = count
            };

            return JsonSerializer.Serialize(result);
        }
    }
}
