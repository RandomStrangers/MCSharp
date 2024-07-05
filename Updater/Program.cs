﻿using System;
using System.Diagnostics;
using static MCSharpUpdater.Updater;
namespace MCSharpUpdater
{
    public class Program
    {
        public const string PrgmName = "MCSharpUpdater";
        public const string Ver = "1.0.0.0";
        public static string Version = Ver;
        public static string path = System.IO.Directory.GetCurrentDirectory();
        public static void Main(string[] args)
        {
            Console.Title = PrgmName + " " + Version;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Updating MCSharp to latest build from:");
            Console.WriteLine(BaseURL);

            try
            {
                Updater.PerformUpdate();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error performing update:");
                Console.WriteLine(ex.ToString());
                Console.ReadKey(false);
                return;
            }
            try
            {
                Process.Start(path + "/MCSharpCLI.exe"); //GUI doesn't work on MONO, so use CLI
            }
            catch(Exception e) 
            {
                Console.WriteLine("Error performing update:");
                Console.WriteLine(e.ToString());
                Console.ReadKey(false);
                return;
            }

            Environment.Exit(0);
        }
    }
}