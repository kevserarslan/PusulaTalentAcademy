using System;
using System.Linq;
using System.Xml.Linq;
using System.Text.Json;

namespace PusulaAssessment
{
    public static class FilterPeopleFromXmlTask
    {
        /// <summary>
        /// XML içinden kişilerden:
        /// - Age > 30
        /// - Department == "IT"
        /// - Salary > 5000
        /// - HireDate < 2019-01-01
        /// şartlarını sağlayanları filtreler ve
        /// {"Names":[...],"TotalSalary":...,"AverageSalary":...,"MaxSalary":...,"Count":N}
        /// biçiminde JSON döndürür.
        /// Uygun kişi yoksa toplamlar 0 olur.
        /// </summary>
        public static string FilterPeopleFromXml(string xmlData)
        {
            if (string.IsNullOrWhiteSpace(xmlData))
            {
                var empty = new { Names = Array.Empty<string>(), TotalSalary = 0m, AverageSalary = 0m, MaxSalary = 0m, Count = 0 };
                return JsonSerializer.Serialize(empty);
            }

            XDocument doc = XDocument.Parse(xmlData);
            DateTime limit = new DateTime(2019, 1, 1);

            var people = doc.Descendants("Person")
                .Select(p => new
                {
                    Name = (string?)p.Element("Name") ?? "",
                    Age = (int?)p.Element("Age") ?? 0,
                    Department = (string?)p.Element("Department") ?? "",
                    Salary = (decimal?)p.Element("Salary") ?? 0m,
                    HireDate = (DateTime?)p.Element("HireDate") ?? DateTime.MinValue
                })
                .Where(x => x.Age > 30
                            && string.Equals(x.Department, "IT", StringComparison.OrdinalIgnoreCase)
                            && x.Salary > 5000m
                            && x.HireDate < limit)
                .ToList();

            var names = people.Select(x => x.Name).OrderBy(n => n).ToList();
            decimal total = people.Sum(x => x.Salary);
            decimal max = people.Count > 0 ? people.Max(x => x.Salary) : 0m;
            int count = people.Count;
            decimal avg = count > 0 ? Math.Round(total / count, 2, MidpointRounding.AwayFromZero) : 0m;

            var result = new
            {
                Names = names,
                TotalSalary = total,
                AverageSalary = avg,
                MaxSalary = max,
                Count = count
            };

            return JsonSerializer.Serialize(result);
        }
    }
}
