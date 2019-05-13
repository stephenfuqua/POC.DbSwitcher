using System;

namespace POC.DbSwitcher
{
    public static class Logger
    {
        public static void WriteYellowLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteLine(string message)
        {
            Console.ResetColor();
            Console.WriteLine(message);            
        }

        public static void WriteSectionHeader(string title)
        {
            WriteYellowLine("-------------------");
            WriteYellowLine(title);
            WriteYellowLine("-------------------");
        }
    }
}
