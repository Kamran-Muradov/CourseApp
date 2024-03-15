using Domain.Models;
using Service.Helpers.Constants;
using Service.Helpers.Extensions;
using Service.Services;
using Service.Services.Interfaces;

namespace CourseApp.Controllers
{
    public class GroupController
    {
        private readonly IGroupService _groupService;

        public GroupController()
        {
            _groupService = new GroupService();
        }

        public void Create()
        {
            ConsoleColor.Cyan.WriteConsole("Enter name:");
        Name: string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Name;
            }

            ConsoleColor.Cyan.WriteConsole("Enter teacher name of this group:");
        Teacher: string teacher = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(teacher))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto Teacher;
            }

            ConsoleColor.Cyan.WriteConsole("Enter room name of this group:");
        Room: string room = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(room))
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
            int id;
            bool isCorrectIdFormat = int.TryParse(idStr, out id);

            if (id < 0)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be negative. Please try again:");
                goto Id;
            }

            if (isCorrectIdFormat)
            {
                try
                {
                    Group updatedGroup = _groupService.GetById(id);

                    if (updatedGroup is null)
                    {
                        ConsoleColor.Red.WriteConsole("There is no group with specified id. Please try again:");
                        goto Id;
                    }

                    ConsoleColor.Cyan.WriteConsole("Enter name (Press Enter if you don't want to change):");
                    string name = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        updatedGroup.Name = name;
                    }

                    ConsoleColor.Cyan.WriteConsole("Enter teacher name of this group (Press Enter if you don't want to change):");
                    string teacher = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(teacher))
                    {
                        updatedGroup.Teacher = teacher;
                    }

                    ConsoleColor.Cyan.WriteConsole("Enter room name of this group: ((Press Enter if you don't want to change)");
                    string room = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(room))
                    {
                        updatedGroup.Room = room;
                    }

                    _groupService.Update(updatedGroup);

                    ConsoleColor.Green.WriteConsole("Data successfully updated");
                }
                catch (Exception ex)
                {
                    ConsoleColor.Red.WriteConsole(ex.Message);
                    goto Id;
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
            int id;
            bool isCorrectIdFormat = int.TryParse(idStr, out id);

            if (id < 0)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be negative. Please try again:");
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
                        goto Id;
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
                ConsoleColor.Red.WriteConsole("There is not any groups to show. Please create one");
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
                    ConsoleColor.Red.WriteConsole("No groups found for this teacher. Please try again:");
                    goto Teacher;
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
        Room: Console.WriteLine("Enter room name:");

            string room = Console.ReadLine().Trim().ToLower();

            try
            {
                var response = _groupService.GetAllWithExpression(m => m.Teacher == room);

                if (response.Count == 0)
                {
                    ConsoleColor.Red.WriteConsole("No groups found for this teacher. Please try again:");
                    goto Room;
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
            int id;
            bool isCorrectIdFormat = int.TryParse(idStr, out id);

            if (id < 0)
            {
                ConsoleColor.Red.WriteConsole("Id cannot be negative. Please try again:");
                goto Id;
            }

            if (isCorrectIdFormat)
            {
                var response = _groupService.GetById(id);

                if (response is null)
                {
                    ConsoleColor.Red.WriteConsole("There is no group with specified id. Please try again");
                    goto Id;
                }

                ConsoleColor.Yellow.WriteConsole($"Id: {response.Id} Name: {response.Name} Teacher: {response.Teacher} Room: {response.Room}");

            }
        }

        public void SearchByName()
        {
            ConsoleColor.Cyan.WriteConsole("Enter search text:");

        SearchText: string searchText = Console.ReadLine().Trim().ToLower();

            var response = _groupService.GetAllWithExpression(m => m.Name.Trim().ToLower().Contains(searchText));

            if (response.Count == 0)
            {
                ConsoleColor.Red.WriteConsole(ResponseMessages.DataNotFound);
                goto SearchText;
            }

            foreach (var item in response)
            {
                ConsoleColor.Yellow.WriteConsole($"Id: {item.Id} Name: {item.Name} Teacher: {item.Teacher} Room: {item.Room}");
            }
        }
    }
}
