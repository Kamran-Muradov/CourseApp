using ConsoleTables;
using Domain.Models;

namespace Service.Helpers.Extensions
{
    public static class GroupExtension
    {
        public static void Print(this Group group)
        {
            Console.WriteLine();

            try
            {
                var table = new ConsoleTable("Id", "Name", "Teacher", "Room", "Student count");

                table.AddRow(group.Id, group.Name, group.Teacher, group.Room, group.StudentCount);

                table.Options.EnableCount = false;

                table.Write();
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }

        public static void PrintAll(this List<Group> groups)
        {
            Console.WriteLine();

            try
            {
                var table = new ConsoleTable("Id", "Name", "Teacher", "Room", "Student count");

                foreach (var item in groups)
                {
                    table.AddRow(item.Id, item.Name, item.Teacher, item.Room, item.StudentCount);
                }

                table.Options.EnableCount = false;

                table.Write();
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }
    }
}
