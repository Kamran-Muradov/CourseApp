using Service.Services.Interfaces;
using Service.Services;
using Service.Helpers.Extensions;
using System.Text.RegularExpressions;
using Service.Helpers.Exceptions;
using Domain.Models;
using Service.Helpers.Constants;
using ConsoleTables;
using System.Xml.Linq;

namespace CourseApp.Controllers
{
    public class StudentController
    {
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;

        public StudentController()
        {
            _groupService = new GroupService();
            _studentService = new StudentService();
        }
        public void Create()
        {
            if (_groupService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("You can't create student without any group. Please create a group first");
                return;
            }

            ConsoleColor.Yellow.WriteConsole("Enter name:");
        Name: string name = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(name))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Name;
            }

            if (!Regex.IsMatch(name, @"^\p{L}{1,20}$"))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidNameFormat);
                goto Name;
            }

            if (name.Length < 3)
            {
                ConsoleColor.Red.WriteConsole("Name must contain at least 3 characters");
                goto Name;
            }

            ConsoleColor.Yellow.WriteConsole("Enter surname:");
        Surname: string surname = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(surname))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Surname;
            }

            if (!Regex.IsMatch(surname, @"^\p{L}{1,20}$"))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidSurnameFormat);
                goto Surname;
            }

            if (surname.Length < 3)
            {
                ConsoleColor.Red.WriteConsole("Surname must contain at least 3 characters");
                goto Surname;
            }

            ConsoleColor.Yellow.WriteConsole("Enter age:");
        Age: string ageStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(ageStr))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Age;
            }

            int age;

            if (!int.TryParse(ageStr, out age))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidAgeFormat + ". Please try again:");
                goto Age;
            }
            else if (age < 1)
            {
                ConsoleColor.Red.WriteConsole("Age cannot be less than 1. Please try again:");
                goto Age;
            }

            ConsoleColor.Yellow.WriteConsole("Enter id of the group you want to add student:");
        Id: string groupIdStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(groupIdStr))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Id;
            }

            int groupId;

            if (!int.TryParse(groupIdStr, out groupId))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidIdFormat + ". Please try again:");
                goto Id;
            }
            else if (groupId < 1)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                goto Id;
            }

            var addedGroup = _groupService.GetById(groupId);

            if (addedGroup is null)
            {
                ConsoleColor.Red.WriteConsole("There is no group with specified id. Please try again:");
                goto Id;
            }

            try
            {
                _studentService.Create(new Student() { Name = name, Surname = surname, Age = age, Group = addedGroup });

                ConsoleColor.Green.WriteConsole("Data successfully added");
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }

        public void Update()
        {
            ConsoleColor.Yellow.WriteConsole("Enter id of the student you want to update:");
        Id: string idStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(idStr))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Id;
            }

            int id;

            if (!int.TryParse(idStr, out id))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidIdFormat + ". Please try again:");
                goto Id;
            }
            else if (id < 1)
            {

                ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                goto Id;
            }

            if (!_studentService.GetAll().Any(m => m.Id == id))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            ConsoleColor.Yellow.WriteConsole("Enter name (Press Enter if you don't want to change):");
        Name: string updatedName = Console.ReadLine().Trim();

            if (!string.IsNullOrEmpty(updatedName))
            {
                if (updatedName.Length < 3)
                {
                    ConsoleColor.Red.WriteConsole("Name must contain at least 3 characters");
                    goto Name;
                }

                if (!Regex.IsMatch(updatedName, @"^\p{L}{1,20}$"))
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidNameFormat);
                    goto Name;
                }
            }

            ConsoleColor.Yellow.WriteConsole("Enter surname (Press Enter if you don't want to change):");
        Surname: string updatedSurname = Console.ReadLine().Trim();

            if (!string.IsNullOrEmpty(updatedSurname))
            {
                if (updatedName.Length < 3)
                {
                    ConsoleColor.Red.WriteConsole("Surname must contain at least 3 characters");
                    goto Surname;
                }
                if (!Regex.IsMatch(updatedSurname, @"^\p{L}{1,20}$"))
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidSurnameFormat);
                    goto Surname;
                }
            }

            ConsoleColor.Yellow.WriteConsole("Enter age (Press Enter if you don't want to change):");
        Age: string ageStr = Console.ReadLine();

            int updatedAge;

            if (!string.IsNullOrWhiteSpace(ageStr))
            {
                if (!int.TryParse(ageStr, out updatedAge))
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidAgeFormat + ". Please try again:");
                    goto Age;
                }
                else if (updatedAge < 1)
                {
                    ConsoleColor.Red.WriteConsole("Age cannot be less than 1. Please try again:");
                    goto Age;
                }
            }

            ConsoleColor.Yellow.WriteConsole("Enter group id you want to switch (Press Enter if you don't want to change):");
        GroupId: string groupIdStr = Console.ReadLine();

            int groupId = 0;

            if (!string.IsNullOrWhiteSpace(groupIdStr))
            {
                if (!int.TryParse(groupIdStr, out groupId))
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidIdFormat + ". Please try again:");
                    goto GroupId;
                }
                else if (groupId < 1)
                {
                    ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                    goto GroupId;
                }
            }

            Domain.Models.Group updatedGroup = null;

            if (groupId > 0)
            {

                updatedGroup = _groupService.GetById(groupId);

                if (updatedGroup is null)
                {
                    ConsoleColor.Red.WriteConsole("There is no group with specified id. Please try again:");
                    goto GroupId;
                }
            }

            try
            {
                _studentService.Update(new() { Id = id, Name = updatedName, Surname = updatedSurname, Group = updatedGroup });

                ConsoleColor.Green.WriteConsole(ResponseMessages.UpdateSuccess);
            }
            catch (Exception ex)
            {
                ConsoleColor.Yellow.WriteConsole(ex.Message);
            }
        }

        public void Delete()
        {
            ConsoleColor.Yellow.WriteConsole("Enter id of the student you want to delete:");
        Id: string idStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(idStr))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Id;
            }

            int id;

            if (!int.TryParse(idStr, out id))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidIdFormat + ". Please try again:");
                goto Id;
            }

            else if (id < 1)
            {

                ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                goto Id;
            }

            try
            {
                var student = _studentService.GetById(id);

                Console.WriteLine("Are you sure you want to delete this student? (Press 'Y' for yes, 'N' for no)");
            DeleteChoice: string deleteChoice = Console.ReadLine().Trim().ToLower();

                if (deleteChoice == "n")
                {
                    return;
                }
                else if (deleteChoice == "y")
                {
                    _studentService.Delete(id);

                    ConsoleColor.Green.WriteConsole(ResponseMessages.DeleteSuccess);
                }
                else
                {
                    ConsoleColor.Red.WriteConsole("Wrong operation. Please try again");
                    goto DeleteChoice;
                }
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }

        public void GetAll()
        {
            var response = _studentService.GetAll();

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any student. Please create one");
                return;
            }

            PrintAll(response);
        }

        public void GetAllByAge()
        {
            ConsoleColor.Yellow.WriteConsole("Enter age: (Press Enter to cancel)");

        Age: string ageStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(ageStr))
            {
                return;
            }

            int age;

            if (!int.TryParse(ageStr, out age))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidAgeFormat + ". Please try again:");
                goto Age;
            }

            else if (age < 1)
            {

                ConsoleColor.Red.WriteConsole("Age cannot be less than 1. Please try again:");
                goto Age;
            }

            List<Student> foundStudents = _studentService.GetAllWithExpression(m => m.Age == age);

            if (foundStudents.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            PrintAll(foundStudents);
        }

        public void GetAllByGroupId()
        {
            ConsoleColor.Yellow.WriteConsole("Enter group id: (Press Enter to cancel)");

        Age: string groupIdStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(groupIdStr))
            {
                return;
            }

            int groupId;

            if (!int.TryParse(groupIdStr, out groupId))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidAgeFormat + ". Please try again:");
                goto Age;
            }

            else if (groupId < 1)
            {

                ConsoleColor.Red.WriteConsole("Group id cannot be less than 1. Please try again:");
                goto Age;
            }

            List<Student> foundStudents = _studentService.GetAllWithExpression(m => m.Group.Id == groupId);

            if (foundStudents.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            PrintAll(foundStudents);
        }

        public void GetById()
        {
            ConsoleColor.Yellow.WriteConsole("Enter id of the student: (Press Enter to cancel)");
        Id: string idStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(idStr))
            {
                return;
            }

            int id;

            if (!int.TryParse(idStr, out id))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidIdFormat + ". Please try again:");
                goto Id;
            }
            else if (id < 1)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                goto Id;
            }

            try
            {
                var response = _studentService.GetById(id);

                Print(response);
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }

        public void SearchByNameOrSurname()
        {
            ConsoleColor.Yellow.WriteConsole("Enter search text: (Press Enter to cancel)");
            string searchText = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            var response = _studentService.GetAllWithExpression(m => m.Name.ToLower().Contains(searchText) || m.Surname.Trim().ToLower().Contains(searchText));

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            PrintAll(response);
        }

        private void Print(Student student)
        {
            Console.WriteLine();
            var table = new ConsoleTable("Id", "Name", "Surname", "Age", "Group id", "Group name");

            table.AddRow(student.Id, student.Name, student.Surname, student.Age, student.Group, student.Group.Name);

            table.Options.EnableCount = false;

            table.Write();
        }

        private void PrintAll(List<Student> students)
        {
            Console.WriteLine();

            var table = new ConsoleTable("Id", "Name", "Surname", "Age", "Group id", "Group name");

            foreach (var item in students)
            {
                table.AddRow(item.Id, item.Name, item.Surname, item.Age, item.Group.Id, item.Group.Name);
            }

            table.Options.EnableCount = false;

            table.Write();
        }
    }
}

