using Domain.Models;
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
        }

        public void GetAll()
        {
            var response = _groupService.GetAll();

            foreach (var item in response)
            {
                Console.WriteLine($"Id: {item.Id} Name: {item.Name} Teacher: {item.Teacher} Room: {item.Room}");
            }
        }
    }
}
