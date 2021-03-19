using System;
using System.Collections.Generic;
using System.IO;

namespace PlanetOfApes.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                FamilyTree familyTree = new FamilyTree();

                Console.WriteLine("Loading instructions from Test file 1..");

                var commandList = ConvertFileToCommands("TestInput1.txt");

                foreach (var command in commandList)
                {
                    command.Execute(familyTree);
                }

                Console.WriteLine("Loading instructions from Test file 2..");

                commandList = ConvertFileToCommands("TestInput2.txt");

                foreach (var command in commandList)
                {
                    command.Execute(familyTree);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static IEnumerable<Command> ConvertFileToCommands(string fileName)
        {
            List<Command> commands = new List<Command>();

            string[] lines = File.ReadAllLines(fileName);

            foreach (var item in lines)
            {
                Command cmd = null;

                string[] tokens = item.Split(" ");

                string commandIndicator = tokens[0];

                if (commandIndicator == "ADD")
                {
                    cmd = new AddCommand()
                    {
                        MothersName = tokens[1],
                        ChildName = tokens[2],
                        IsChildMale = tokens[3] != "Female"
                    };
                }
                else if(commandIndicator == "GET")
                {
                    cmd = new GetCommand()
                    {
                        MemberName = tokens[1],
                        RelationShipToGet = tokens[2],
                    };
                }

                if (cmd != null)
                {
                    commands.Add(cmd);
                }
            }

            return commands;
        }
    }



  


}
