using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Fluctuface;

namespace Fluctuface.Server
{
    class FluctuantVariable
    {
        public Fluctuant Fluctuant;
        public FieldInfo FieldInfo;
    }

    class Program
    {
        static readonly HttpClient httpClient;

        static Program()
        {
            httpClient = new HttpClient(new HttpClientHandler(), true);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Fluctuface.Server.Program");

            string folderPath = Directory.GetCurrentDirectory();
            if (args.Length > 0)
            {
                folderPath = args[0];
            }
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            string searchPattern = "*.dll";
            var fluctuants = new List<FluctuantVariable>();

            foreach (FileInfo file in directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly))
            {
                fluctuants.AddRange(GetFluctuants(file.FullName));
            }


            Console.ReadLine();
        }

        private static List<FluctuantVariable> GetFluctuants(string dllPath)
        {
            Assembly assembly = Assembly.LoadFile(dllPath);
            var fluctuants = new List<FluctuantVariable>();

            if (assembly.GetCustomAttribute<FluctuantAssembly>() != null)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        var fluct = field.GetCustomAttribute<Fluctuant>();

                        if (fluct != null)
                        {
                            Console.WriteLine("Found a fluct!");
                            var fluctuantVariable = new FluctuantVariable
                            {
                                FieldInfo = field,
                                Fluctuant = fluct,
                            };
                            fluctuants.Add(fluctuantVariable);
                        }
                    }
                }

            }

            //Type type = assembly.GetType(ClassName);
            //return Type.GetType(type.AssemblyQualifiedName).Get
            //GetProperties().Select(a => a.ToString()).ToList();
            return fluctuants;
        }
    }
}
