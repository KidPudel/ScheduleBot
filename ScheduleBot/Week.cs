using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleBot
{
    public class Week
    {
        public const string Monday = "Понедельник";
        public const string Tuesday = "Вторник";
        public const string Wednesday = "Среда";
        public const string Thursday = "Четверг";
        public const string Friday = "Пятница";

        public static bool IsValid(string day)
        {
            return day switch
            {
                Monday => true,
                Tuesday => true,
                Wednesday => true,
                Thursday => true,
                Friday => true,
                _ => false
            };
        }

        public static string GetSchedule(string day)
        {
            var path = $"C:/Users/ikupc/source/repos/ScheduleBot/ScheduleBot/Schedule/{day}.txt";
            using FileStream fs = new FileStream(path, FileMode.Open);
            using StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            return sr.ReadToEnd();
        }
    }
}
