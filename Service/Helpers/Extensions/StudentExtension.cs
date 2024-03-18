using ConsoleTables;
using Domain.Models;

namespace Service.Helpers.Extensions
{
    public static class StudentExtension
    {
        public static void Print(this Student student)
        {
            Console.WriteLine();

            var table = new ConsoleTable("Id", "Name", "Surname", "Age", "Group", "Teacher");

            table.AddRow(student.Id, student.Name, student.Surname, student.Age, student.Group.Name, student.Group.Teacher);

            table.Options.EnableCount = false;

            table.Write();
        }

        public static void PrintAll(this List<Student> students)
        {
            Console.WriteLine();

            var table = new ConsoleTable("Id", "Name", "Surname", "Age", "Group", "Teacher");

            foreach (var item in students)
            {
                table.AddRow(item.Id, item.Name, item.Surname, item.Age, item.Group.Name, item.Group.Teacher);
            }

            table.Options.EnableCount = false;

            table.Write();
        }
    }
}
