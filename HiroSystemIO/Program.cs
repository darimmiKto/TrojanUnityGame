using HiroSystemIO.ServerConfuration;
using System;
using System.Threading;

namespace HiroSystemIO
{
    internal class Program
    {
        private static Thread thread_console;
        static void Main(string[] args)
        {
            Console.Title = "Сервер";
            Console.ForegroundColor = ConsoleColor.Green;

            thread_console = new Thread(new ThreadStart(ConsoleThread));
            thread_console.Start();

            NetworkConfiguration.InitNetwork(); 
            NetworkConfiguration.socket.StartListening(5444, 5, 1); 

            Console.WriteLine("Сервер запущен!");
        }

        private static void ConsoleThread()
        {
            while (true)
            {

            }
        }
    }
}
