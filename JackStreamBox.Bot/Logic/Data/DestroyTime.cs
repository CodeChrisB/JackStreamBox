using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Data
{
    public class DestroyTime
    {
        public static TimeSpan INSTANT = TimeSpan.FromSeconds(0);
        public static TimeSpan ReallyFast = TimeSpan.FromSeconds(2);
        public static TimeSpan FAST = TimeSpan.FromSeconds(4);
        public static TimeSpan NORMAL = TimeSpan.FromSeconds(10);
        public static TimeSpan SLOW = TimeSpan.FromSeconds(20);
        public static TimeSpan REALLYSLOW = TimeSpan.FromSeconds(60);
        public static TimeSpan ULTRASLOW = TimeSpan.FromMinutes(2);
    }
}
