using ConsoleTables;
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
            ConsoleColor.Yellow.WriteConsole("Enter name:");
        Name: string name = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(name))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Name;
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

            if (teacher.Length < 3)
            {
                ConsoleColor.Red.WriteConsole("Name must contain at least 3 characters");
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
            ConsoleColor.Yellow.WriteConsole("Enter id of the group you want to update:");
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
            else
            {
                if (id < 1)
                {
                    ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                    goto Id;
                }

                if (!_groupService.GetAll().Any(m => m.Id == id))
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

                    if (updatedTeacher.Length < 3)
                    {
                        ConsoleColor.Red.WriteConsole("Name must contain at least 3 characters");
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
        }

        public void Delete()
        {
            ConsoleColor.Yellow.WriteConsole("Enter id of the group you want to delete:");
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
            else
            {
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

                    if (deleteChoice == "n")
                    {
                        return;
                    }
                    else if (deleteChoice == "y")
                    {
                        _groupService.Delete(id);

                        List<Student> students = _studentService.GetAllWithExpression(m => m.Group.Id == id);

                        foreach (var item in students)
                        {
                            _studentService.Delete(item.Id);
                        }

                        ConsoleColor.Green.WriteConsole(ResponseMessages.DeleteSuccess);

                    }
                    else
                    {
                        ConsoleColor.Red.WriteConsole("Wrong operation. Please try again:");
                        goto DeleteChoice;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleColor.Red.WriteConsole(ex.Message);
                }
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

            PrintAll(response);
        }

        public void GetAllByTeacher()
        {
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

            PrintAll(response);
        }

        public void GetAllByRoom()
        {
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

            PrintAll(response);
        }

        public void GetById()
        {
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
            else if (id < 1)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                goto Id;
            }

            try
            {
                var response = _groupService.GetById(id);

                Print(response);
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
            }
        }

        public void SearchByName()
        {
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

            PrintAll(response);
        }

        private void Print(Domain.Models.Group group)
        {
            Console.WriteLine();

            var table = new ConsoleTable("Id", "Name", "Teacher", "Room");

            table.AddRow(group.Id, group.Name, group.Teacher, group.Room);

            table.Options.EnableCount = false;

            table.Write();
        }

        private void PrintAll(List<Domain.Models.Group> groups)
        {
            Console.WriteLine();

            var table = new ConsoleTable("Id", "Name", "Teacher", "Room");

            foreach (var item in groups)
            {
                table.AddRow(item.Id, item.Name, item.Teacher, item.Room);
            }

            table.Options.EnableCount = false;

            table.Write();
        }
    }
}
