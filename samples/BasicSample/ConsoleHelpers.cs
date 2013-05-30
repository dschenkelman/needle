using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicSample
{
    public static class ConsoleHelpers
    {
        public static void WriteMessage(string message, MessageKind kind) 
        {
            switch (kind)
            {
                case MessageKind.Action:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case MessageKind.Warning:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    break;
            }

            string finalMessage = String.Format("{0}: {1}", kind.ToString().ToUpper(), message);
            Console.WriteLine(finalMessage);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

    public enum MessageKind 
    {
        Action,
        Warning
    }
}
