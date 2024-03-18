using Service.Services.Interfaces;
using Service.Services;
using Service.Helpers.Extensions;
using System.Text.RegularExpressions;
using Domain.Models;
using Service.Helpers.Constants;

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

            ConsoleColor.Yellow.WriteConsole("Enter name: (Press Enter to cancel)");
        Name: string name = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            if (!Regex.IsMatch(name, @"^\p{L}{1,20}$"))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidNameFormat);
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
            else if (age < 15 || age > 50)
            {
                ConsoleColor.Red.WriteConsole("Age must be between 15 and 50. Please try again:");
                goto Age;
            }

            Console.WriteLine();
            var groups = _groupService.GetAll();
            ConsoleColor.Yellow.WriteConsole("Groups:");
            groups.PrintAll();

            ConsoleColor.Yellow.WriteConsole("Enter id of the group you want to add student:");
        GroupId: string groupIdStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(groupIdStr))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto GroupId;
            }

            int groupId;

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

            Domain.Models.Group addedGroup;

            try
            {
                addedGroup = _groupService.GetById(groupId);
            }
            catch (Exception)
            {
                ConsoleColor.Red.WriteConsole("There is no group with specified id. Please try again:");
                goto GroupId;
            }

            try
            {
                addedGroup.StudentCount++;

                if (addedGroup.StudentCount > 3)
                {
                    if (groups.All(m => m.StudentCount >= 3))
                    {
                        ConsoleColor.Red.WriteConsole("There is not any empty group. Please create a new one");
                        addedGroup.StudentCount--;
                        return;
                    }
                    else
                    {
                        ConsoleColor.Red.WriteConsole("Group can have maximum 3 students. Please choose another group:");
                        addedGroup.StudentCount--;
                        goto GroupId;
                    }
                }

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
            var students = _studentService.GetAll();

            if (students.Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any student. Please create one");
                return;
            }

            Console.WriteLine();
            ConsoleColor.Yellow.WriteConsole("Students:");
            students.PrintAll();

            ConsoleColor.Yellow.WriteConsole("Enter id of the student you want to update: (Press Enter to cancel)");
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

            if (!_studentService.GetAll().Any(m => m.Id == id))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            ConsoleColor.Yellow.WriteConsole("Enter name (Press Enter if you don't want to change):");
        Name: string updatedName = Console.ReadLine().Trim();

            if (!string.IsNullOrEmpty(updatedName))
            {
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
                if (!Regex.IsMatch(updatedSurname, @"^\p{L}{1,20}$"))
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidSurnameFormat);
                    goto Surname;
                }
            }

            ConsoleColor.Yellow.WriteConsole("Enter age (Press Enter if you don't want to change):");
        Age: string ageStr = Console.ReadLine();

            int updatedAge = 0;

            if (!string.IsNullOrWhiteSpace(ageStr))
            {
                if (!int.TryParse(ageStr, out updatedAge))
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidAgeFormat + ". Please try again:");
                    goto Age;
                }
                else if (updatedAge < 15 || updatedAge > 50)
                {
                    ConsoleColor.Red.WriteConsole("Age must be between 15 and 50. Please try again:");
                    goto Age;
                }
            }

            Console.WriteLine();
            var groups = _groupService.GetAll();
            ConsoleColor.Yellow.WriteConsole("Groups:");
            groups.PrintAll();

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
                try
                {
                    updatedGroup = _groupService.GetById(groupId);
                }
                catch (Exception)
                {
                    ConsoleColor.Red.WriteConsole("There is no group with specified id. Please try again:");
                    goto GroupId;
                }
            }

            try
            {
                if (updatedGroup is not null)
                {
                    updatedGroup.StudentCount++;

                    if (updatedGroup.StudentCount > 3)
                    {
                        if (groups.All(m => m.StudentCount >= 3))
                        {
                            ConsoleColor.Red.WriteConsole("There is not any empty group. Please create a new one");
                            updatedGroup.StudentCount--;
                            goto GroupId;
                        }
                        else
                        {
                            ConsoleColor.Red.WriteConsole("Group can have maximum 3 students. Please choose another group:");
                            updatedGroup.StudentCount--;
                            goto GroupId;
                        }
                    }
                    else
                    {
                        var oldGroup = _studentService.GetById(id).Group;
                        oldGroup.StudentCount--;
                    }

                }

                _studentService.Update(new() { Id = id, Name = updatedName, Surname = updatedSurname, Age = updatedAge, Group = updatedGroup });

                ConsoleColor.Green.WriteConsole(ResponseMessages.UpdateSuccess);
            }
            catch (Exception ex)
            {
                ConsoleColor.Yellow.WriteConsole(ex.Message);
            }
        }

        public void Delete()
        {
            var students = _studentService.GetAll();

            if (students.Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any student. Please create one");
                return;
            }

            Console.WriteLine();
            ConsoleColor.Yellow.WriteConsole("Students:");
            students.PrintAll();

            ConsoleColor.Yellow.WriteConsole("Enter id of the student you want to delete: (Press Enter to cancel)");
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
                var student = _studentService.GetById(id);

                Console.WriteLine("Are you sure you want to delete this student? (Press 'Y' for yes, 'N' for no)");
            DeleteChoice: string deleteChoice = Console.ReadLine().Trim().ToLower();

                if (deleteChoice == "n")
                {
                    return;
                }
                else if (deleteChoice == "y")
                {
                    var oldGroup = _studentService.GetById(id).Group;
                    oldGroup.StudentCount--;

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

            response.PrintAll();
        }

        public void GetAllByAge()
        {
            if (_studentService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any student. Please create one");
                return;
            }

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
                ConsoleColor.Red.WriteConsole("Age must be between greater than 0. Please try again:");
                goto Age;
            }

            List<Student> foundStudents = _studentService.GetAllWithExpression(m => m.Age == age);

            if (foundStudents.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            foundStudents.PrintAll();
        }

        public void GetAllByGroupId()
        {
            if (_studentService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any student. Please create one");
                return;
            }

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

            foundStudents.PrintAll();
        }

        public void GetById()
        {
            if (_studentService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any student. Please create one");
                return;
            }

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

                response.Print();
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }

        public void SearchByNameOrSurname()
        {
            if (_studentService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any student. Please create one");
                return;
            }

            ConsoleColor.Yellow.WriteConsole("Enter search text: (Press Enter to cancel)");
            string searchText = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            var response = _studentService.GetAllWithExpression(m => m.Name.ToLower().Contains(searchText) || m.Surname.ToLower().Contains(searchText));

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            response.PrintAll();
        }
    }
}

