using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartScreen
{
    public class Logger
    {
        public static void info(object message)
        {
            Console.WriteLine($"[INFO] {message}");
        }

        public static void warn(object message)
        {
            Console.WriteLine($"[WARN] {message}");
        }

        public static void error(object message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }

        public static void debug(object message)
        {
            #if DEBUG
            Console.WriteLine($"[DEBUG] {message}");
            #endif
        }
    }
}
