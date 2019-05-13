using System;

namespace POC.DbSwitcher.Console
{
    public static class Logger
    {
        public static void WriteYellowLine(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(message);
            System.Console.ResetColor();
        }

        public static void WriteLine(string message)
        {
            System.Console.ResetColor();
            System.Console.WriteLine(message);            
        }

        public static void ErrorLine(string message)
        {
            System.Console.ResetColor();
            System.Console.Error.WriteLine(message);
        }

        public static void WriteSectionHeader(string title)
        {
            WriteYellowLine("-------------------");
            WriteYellowLine(title);
            WriteYellowLine("-------------------");
        }
    }
}
