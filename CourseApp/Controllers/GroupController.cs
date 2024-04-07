using Domain.Models;
using Service.Helpers.Constants;
using Service.Helpers.Extensions;
using Service.Services;
using Service.Services.Interfaces;
using System.Text.RegularExpressions;

namespace CourseApp.Controllers
{
    public class GroupController
    {
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;

        public GroupController()
        {
            _groupService = new GroupService();
            _studentService = new StudentService();
        }

        public void Create()
        {
            ConsoleColor.Yellow.WriteConsole("Enter name: (Press Enter to cancel)");
        Name: string name = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            if (_groupService.GetAll().Any(m => m.Name.ToLower() == name.ToLower()))
            {
                ConsoleColor.Red.WriteConsole("Group with this name already exists");
                goto Name;
            }

            ConsoleColor.Yellow.WriteConsole("Enter teacher name of this group:");
        Teacher: string teacher = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(teacher))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Teacher;
            }

            if (!Regex.IsMatch(teacher, @"^[\p{L}]+(?:\s[\p{L}]+)?$"))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidNameFormat);
                goto Teacher;
            }

            ConsoleColor.Yellow.WriteConsole("Enter room name of this group:");
        Room: string room = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(room))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Room;
            }

            try
            {
                _groupService.Create(new Domain.Models.Group { Name = name, Teacher = teacher, Room = room });

                ConsoleColor.Green.WriteConsole(ResponseMessages.AddSuccess);
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }

        public void Update()
        {
            if (_groupService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any group. Please create one");
                return;
            }

            Console.WriteLine();
            ConsoleColor.Yellow.WriteConsole("Groups:");
            _groupService.GetAll().PrintAll();

            ConsoleColor.Yellow.WriteConsole("Enter id of the group you want to update: (Press Enter to cancel)");
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

            if (id < 1)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                goto Id;
            }

            if (_groupService.GetAll().All(m => m.Id != id))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            ConsoleColor.Yellow.WriteConsole("Enter name (Press Enter if you don't want to change):");
            string updatedName = Console.ReadLine().Trim();

            ConsoleColor.Yellow.WriteConsole("Enter teacher name of this group (Press Enter if you don't want to change):");
            Teacher: string updatedTeacher = Console.ReadLine().Trim();

            if (!string.IsNullOrEmpty(updatedTeacher))
            {
                if (!Regex.IsMatch(updatedTeacher, @"^[\p{L}]+(?:\s[\p{L}]+)?$"))
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidNameFormat);
                    goto Teacher;
                }
            }

            ConsoleColor.Yellow.WriteConsole("Enter room name of this group (Press Enter if you don't want to change):");
            string updatedRoom = Console.ReadLine().Trim();

            try
            {
                _groupService.Update(new() { Id = id, Name = updatedName, Teacher = updatedTeacher, Room = updatedRoom });

                ConsoleColor.Green.WriteConsole(ResponseMessages.UpdateSuccess);
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }

        public void Delete()
        {
            if (_groupService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any group. Please create one");
                return;
            }

            Console.WriteLine();
            ConsoleColor.Yellow.WriteConsole("Groups:");
            _groupService.GetAll().PrintAll();

            ConsoleColor.Yellow.WriteConsole("Enter id of the group you want to delete: (Press Enter to cancel)");
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

            if (id < 1)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                goto Id;
            }

            try
            {
                var group = _groupService.GetById(id);

                ConsoleColor.Yellow.WriteConsole("Are you sure you want to delete this group? Group and its students will be deleted (Press 'Y' for yes, 'N' for no)");
                DeleteChoice: string deleteChoice = Console.ReadLine().Trim().ToLower();

                switch (deleteChoice)
                {
                    case "n":
                        return;
                    case "y":
                    {
                        _groupService.Delete(id);

                        List<Student> students = _studentService.GetAllWithExpression(m => m.Group.Id == id);

                        foreach (var item in students)
                        {
                            _studentService.Delete(item.Id);
                        }

                        ConsoleColor.Green.WriteConsole(ResponseMessages.DeleteSuccess);
                        break;
                    }
                    default:
                        ConsoleColor.Red.WriteConsole("Wrong operation. Please try again:");
                        goto DeleteChoice;
                }
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
                goto Id;
            }
        }

        public void GetAll()
        {
            var response = _groupService.GetAll();

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any group. Please create one");
                return;
            }

            response.PrintAll();
        }

        public void GetAllByTeacher()
        {
            if (_groupService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any group. Please create one");
                return;
            }

        Teacher: ConsoleColor.Yellow.WriteConsole("Enter teacher name:(Press Enter to cancel)");

            string teacher = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(teacher))
            {
                return;
            }

            var response = _groupService.GetAllWithExpression(m => m.Teacher.ToLower() == teacher);

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            response.PrintAll();
        }

        public void GetAllByRoom()
        {
            if (_groupService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any group. Please create one");
                return;
            }

            ConsoleColor.Yellow.WriteConsole("Enter room name: (Press Enter to cancel)");

            string room = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(room))
            {
                return;
            }

            var response = _groupService.GetAllWithExpression(m => m.Room.ToLower() == room);

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            response.PrintAll();
        }

        public void GetById()
        {
            if (_groupService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any group. Please create one");
                return;
            }

            ConsoleColor.Yellow.WriteConsole("Enter id of the group: (Press Enter to cancel)");
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

            if (id < 1)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                goto Id;
            }

            try
            {
                var response = _groupService.GetById(id);

                response.Print();
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }

        public void SearchByName()
        {
            if (_groupService.GetAll().Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any group. Please create one");
                return;
            }

            ConsoleColor.Yellow.WriteConsole("Enter search text: (Press Enter to cancel)");

            string searchText = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            var response = _groupService.GetAllWithExpression(m => m.Name.ToLower().Contains(searchText));

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            response.PrintAll();
        }
    }
}
