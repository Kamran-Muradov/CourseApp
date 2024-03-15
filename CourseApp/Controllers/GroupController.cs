using Domain.Models;
using Service.Helpers.Constants;
using Service.Helpers.Exceptions;
using Service.Helpers.Extensions;
using Service.Services;
using Service.Services.Interfaces;

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
        Name: string name = Console.ReadLine().Trim().ToLower();

            if (string.IsNullOrEmpty(name))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Name;
            }

            ConsoleColor.Cyan.WriteConsole("Enter teacher name of this group:");
        Teacher: string teacher = Console.ReadLine().Trim().ToLower();
            if (string.IsNullOrEmpty(teacher))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Teacher;
            }

            ConsoleColor.Cyan.WriteConsole("Enter room name of this group:");
        Room: string room = Console.ReadLine().Trim().ToLower();
            if (string.IsNullOrEmpty(room))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Room;
            }
            try
            {
                _groupService.Create(new Group { Name = name, Teacher = teacher, Room = room });

                ConsoleColor.Green.WriteConsole("Data successfully added");
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.WriteConsole(ex.Message);
                goto Name;
            }
        }

        public void Update()///
        {
            ConsoleColor.Cyan.WriteConsole("Enter id of the group you want to update:");
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
                    Group updatedGroup = _groupService.GetById(id);

                    ConsoleColor.Cyan.WriteConsole("Enter name (Press Enter if you don't want to change):");
                    string name = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        updatedGroup.Name = name.Trim().ToLower();
                    }

                    ConsoleColor.Cyan.WriteConsole("Enter teacher name of this group (Press Enter if you don't want to change):");
                    string teacher = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(teacher))
                    {
                        updatedGroup.Teacher = teacher.Trim().ToLower();
                    }

                    ConsoleColor.Cyan.WriteConsole("Enter room name of this group (Press Enter if you don't want to change):");
                    string room = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(room))
                    {
                        updatedGroup.Room = room.Trim().ToLower();
                    }

                    _groupService.Update(updatedGroup);

                    ConsoleColor.Green.WriteConsole("Data successfully updated");
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
            else
            {
                ConsoleColor.Red.WriteConsole("Id format is wrong. Please try again:");
                goto Id;
            }
        } ///

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
            bool isCorrectIdFormat = int.TryParse(idStr, out id);

            if (id < 1)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be less than 1. Please try again:");
                goto Id;
            }

            if (isCorrectIdFormat)
            {
                Console.WriteLine("Are you sure you want to delete this group? (Press 'Y' for yes, 'N' for no)");
            DeleteChoice: string deleteChoice = Console.ReadLine().Trim().ToLower();

                if (deleteChoice == "n")
                {
                    return;
                }
                else if (deleteChoice == "y")
                {
                    try
                    {
                        _groupService.Delete(id);
                        ConsoleColor.Green.WriteConsole("Data successfully deleted");
                    }
                    catch (Exception ex)
                    {
                        ConsoleColor.Red.WriteConsole(ex.Message);
                        return;
                    }
                }
                else
                {
                    ConsoleColor.Red.WriteConsole("Wrong operation. Please try again");
                    goto DeleteChoice;
                }
            }
            else
            {
                ConsoleColor.Red.WriteConsole("Id format is wrong. Please try again:");
                goto Id;
            }
        } ///

        public void GetAll()
        {
            var response = _groupService.GetAll();

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole("There is not any group. Please create one");
                return;
            }

            foreach (var item in response)
            {
                ConsoleColor.Yellow.WriteConsole($"Id: {item.Id} Name: {item.Name} Teacher: {item.Teacher} Room: {item.Room}");
            }
        }

        public void GetAllByTeacher()
        {
        Teacher: Console.WriteLine("Enter teacher name:");

            string teacher = Console.ReadLine().Trim().ToLower();

            try
            {
                var response = _groupService.GetAllWithExpression(m => m.Teacher == teacher);

                if (response.Count == 0)
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                    return;
                }

                foreach (var item in response)
                {
                    ConsoleColor.Yellow.WriteConsole($"Id: {item.Id} Name: {item.Name} Teacher: {item.Teacher} Room: {item.Room}");
                }
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
                var response = _groupService.GetAllWithExpression(m => m.Teacher == room);

                if (response.Count == 0)
                {
                    ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                    return;
                }

                foreach (var item in response)
                {
                    ConsoleColor.Yellow.WriteConsole($"Id: {item.Id} Name: {item.Name} Teacher: {item.Teacher} Room: {item.Room}");
                }
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
                    var response = _groupService.GetById(id);

                    ConsoleColor.Yellow.WriteConsole($"Id: {response.Id} Name: {response.Name} Teacher: {response.Teacher} Room: {response.Room}");
                }
                catch (Exception ex)
                {
                    ConsoleColor.Red.WriteConsole(ex.Message);
                    return;
                }
            }
            else
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.InvalidIdFormat + ". Please try again:");
                goto Id;
            }
        }

        public void SearchByName()
        {
            ConsoleColor.Cyan.WriteConsole("Enter search text:");

            string searchText = Console.ReadLine().Trim().ToLower();

            var response = _groupService.GetAllWithExpression(m => m.Name.Trim().ToLower().Contains(searchText));

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                return;
            }

            foreach (var item in response)
            {
                ConsoleColor.Yellow.WriteConsole($"Id: {item.Id} Name: {item.Name} Teacher: {item.Teacher} Room: {item.Room}");
            }
        }
    }
}
