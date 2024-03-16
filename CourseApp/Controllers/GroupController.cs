using Domain.Models;
using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
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
            ConsoleColor.Cyan.WriteConsole("Enter name:");
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

            ConsoleColor.Cyan.WriteConsole("Enter teacher name of this group:");
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

            ConsoleColor.Cyan.WriteConsole("Enter room name of this group:");
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
                goto Name;
            }
        }

        public void Update()
        {
            ConsoleColor.Cyan.WriteConsole("Enter id of the group you want to update:");
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
                    Domain.Models.Group updatedGroup = _groupService.GetById(id);

                    if (updatedGroup != null)
                        Print(updatedGroup);

                    ConsoleColor.Cyan.WriteConsole("Enter name (Press Enter if you don't want to change):");
                    string name = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        updatedGroup.Name = name.Trim();
                    }

                    ConsoleColor.Cyan.WriteConsole("Enter teacher name of this group (Press Enter if you don't want to change):");
                Teacher: string teacher = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(teacher))
                    {
                        updatedGroup.Teacher = teacher.Trim();
                    }

                    if (!Regex.IsMatch(teacher, @"^[\p{L}]+(?:\s[\p{L}]+)?$"))
                    {
                        ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidNameFormat);
                        goto Teacher;
                    }

                    ConsoleColor.Cyan.WriteConsole("Enter room name of this group (Press Enter if you don't want to change):");
                    string room = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(room))
                    {
                        updatedGroup.Room = room.Trim();
                    }

                    _groupService.Update(updatedGroup);

                    ConsoleColor.Green.WriteConsole(ResponseMessages.UpdateSuccess);
                }
                catch (NotFoundException ex)
                {
                    ConsoleColor.Red.WriteConsole(ex.Message);
                    return;
                }
                catch (Exception ex)
                {
                    ConsoleColor.Red.WriteConsole(ex.Message);
                }
            }
        }

        public void Delete()
        {
            ConsoleColor.Cyan.WriteConsole("Enter id of the group you want to delete:");
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
                    Console.WriteLine("Are you sure you want to delete this group? Group and its students will be deleted (Press 'Y' for yes, 'N' for no)");

                DeleteChoice: string deleteChoice = Console.ReadLine().Trim().ToLower();

                    if (deleteChoice == "n")
                    {
                        return;
                    }
                    else if (deleteChoice == "y")
                    {
                        _groupService.Delete(id);

                        List<Student> students = _studentService.GetAllWithExpression(m => m.Id == id);

                        foreach (var item in students)
                        {
                            _studentService.Delete(item.Id);
                        }

                        ConsoleColor.Green.WriteConsole("Data successfully deleted");
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
        Teacher: Console.WriteLine("Enter teacher name:");

            string teacher = Console.ReadLine().Trim().ToLower();

            try
            {
                var response = _groupService.GetAllWithExpression(m => m.Teacher.Trim().ToLower() == teacher);

                if (response.Count == 0)
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                    return;
                }

                PrintAll(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetAllByRoom()
        {
            Console.WriteLine("Enter room name:");

            string room = Console.ReadLine().Trim().ToLower();

            try
            {
                var response = _groupService.GetAllWithExpression(m => m.Room.Trim().ToLower() == room);

                if (response.Count == 0)
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                    return;
                }

                PrintAll(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetById()
        {
            ConsoleColor.Cyan.WriteConsole("Enter id of the group:");
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
            ConsoleColor.Cyan.WriteConsole("Enter search text:");

            string searchText = Console.ReadLine().Trim().ToLower();

            var response = _groupService.GetAllWithExpression(m => m.Name.Trim().ToLower().Contains(searchText));

            if (response.Count == 0 || string.IsNullOrWhiteSpace(searchText))
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            PrintAll(response);
        }

        private void Print(Domain.Models.Group group)
        {
            ConsoleColor.Yellow.WriteConsole($"Id: {group.Id}, Name: {group.Name}, Teacher: {group.Teacher}, Room: {group.Room}");

        }

        private void PrintAll(List<Domain.Models.Group> groups)
        {
            foreach (var item in groups)
            {
                Print(item);
            }
        }
    }
}
