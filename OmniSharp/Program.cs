﻿using System;
using NDesk.Options;
using Nancy.Hosting.Self;
using OmniSharp.AutoComplete;
using OmniSharp.Solution;

namespace OmniSharp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            bool showHelp = false;
            string solutionPath = null;

            int port = 2000;

            var p = new OptionSet
                        {
                            {
                                "s|solution=", "The path to the solution file",
                                v => solutionPath = v
                            },
                            {
                                "p|port=", "Port number to listen on",
                                (int v) => port = v
                            },
                            {
                                "h|help", "show this message and exit",
                                v => showHelp = v != null
                            },
                        };

            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try 'omnisharp --help' for more information.");
                return;
            }

            showHelp |= solutionPath == null;

            if (showHelp)
            {
                ShowHelp(p);
                return;
            }

            var solution = new CSharpSolution(solutionPath);

            var nancyHost = new NancyHost(new Bootstrapper(solution), new Uri("http://localhost:" + port));
            
            nancyHost.Start();
 
            Console.ReadLine();
            nancyHost.Stop();
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: greet [OPTIONS]+ message");
            Console.WriteLine("Greet a list of individuals with an optional message.");
            Console.WriteLine("If no message is specified, a generic greeting is used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
