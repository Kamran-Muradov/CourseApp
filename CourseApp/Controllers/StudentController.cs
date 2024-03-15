using Service.Services.Interfaces;
using Service.Services;
using Service.Helpers.Extensions;
using System.Text.RegularExpressions;
using Service.Helpers.Exceptions;
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

            ConsoleColor.Cyan.WriteConsole("Enter name:");
        Name: string name = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(name))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Name;
            }

            if (!Regex.IsMatch(name, "^[a-zA-Z](?:[a-zA-Z]*[a-zA-Z])?$"))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidNameFormat);
                goto Name;
            }

            ConsoleColor.Cyan.WriteConsole("Enter surname:");
        Surname: string surname = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(surname))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Surname;
            }

            if (!Regex.IsMatch(surname, "^[a-zA-Z](?:[a-zA-Z]*[a-zA-Z])?$"))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidSurnameFormat);
                goto Surname;
            }

            ConsoleColor.Cyan.WriteConsole("Enter age:");
        Age: string ageStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(ageStr))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Age;
            }

            int age;

            bool isCorrectAgeFormat = int.TryParse(ageStr, out age);

            if (isCorrectAgeFormat)
            {
                if (age < 0)
                {
                    ConsoleColor.Red.WriteConsole("Age cannot be negative. Please try again:");
                    goto Age;
                }
                ConsoleColor.Cyan.WriteConsole("Enter id of the group you want to add student:");

            Id: string idStr = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(idStr))
                {
                    ConsoleColor.Red.WriteConsole("Input can't be empty");
                    goto Id;
                }
                int id;

                bool isCorrectIdFormat = int.TryParse(idStr, out id);

                if (isCorrectIdFormat)
                {
                    if (id < 1)
                    {
                        ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                        goto Id;
                    }

                    try
                    {
                        var addedGroup = _groupService.GetById(id);

                        _studentService.Create(new Student() { Name = name, Surname = surname, Age = age, Group = addedGroup });

                        ConsoleColor.Green.WriteConsole("Data successfully added");
                    }
                    catch (NotFoundException)
                    {
                        ConsoleColor.Red.WriteConsole("There is no group with specified id. Please try again:");
                        goto Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidIdFormat + ". Please try again:");
                    goto Id;
                }
            }
            else
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidAgeFormat + ". Please try again:");
                goto Age;
            }
        }

        public void Update()
        {
            ConsoleColor.Cyan.WriteConsole("Enter id of the student you want to update:");
        Id: string idStr = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(idStr))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Id;
            }

            int id;

            bool isCorrectIdFormat = int.TryParse(idStr, out id);

            if (isCorrectIdFormat)
            {
                if (id < 1)
                {
                    ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                    goto Id;
                }

                try
                {
                    Student updatedStudent = _studentService.GetById(id);

                    ConsoleColor.Yellow.WriteConsole($"Name : {updatedStudent.Name}, Surname: {updatedStudent.Surname}, Age: {updatedStudent.Age}, Group id: {updatedStudent.Group.Id}");

                    ConsoleColor.Cyan.WriteConsole("Enter name (Press Enter if you don't want to change):");
                    string name = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        updatedStudent.Name = name.Trim().ToLower();
                    }

                    ConsoleColor.Cyan.WriteConsole("Enter surname (Press Enter if you don't want to change):");
                    string surname = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(surname))
                    {
                        updatedStudent.Name = surname.Trim().ToLower();
                    }

                    ConsoleColor.Cyan.WriteConsole("Enter age (Press Enter if you don't want to change):");

                Age: string ageStr = Console.ReadLine();

                    int age;

                    bool isCorrectAgeFormat = int.TryParse(ageStr, out age);

                    if (isCorrectAgeFormat)
                    {
                        if (age < 0)
                        {
                            ConsoleColor.Red.WriteConsole("Age cannot be negative. Please try again:");
                            goto Age;
                        }

                        updatedStudent.Age = age;


                    }
                }
                catch (NotFoundException ex)
                {
                    ConsoleColor.Red.WriteConsole(ex.Message);
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
