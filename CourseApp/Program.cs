
using CourseApp.Controllers;
using Domain.Models;
using Repository.Data;
using Service.Helpers.Enums;
using Service.Helpers.Extensions;
using Service.Services;

//AppDbContext<Group>.datas = new List<Group> {
//    new()
//    {
//        Id = 1,
//        Name="kamr"
//    },

//    new()
//    {
//        Id = 2,
//        Name="salam"
//    },
//    new()
//    {
//        Id = 3,
//        Name="sagol"
//    }
//};

//foreach (var item in AppDbContext<Group>.datas)
//{
//    Console.WriteLine(item.Id + " " + item.Name);
//}
//GroupService groupService = new GroupService();
//groupService.Update(new Group { Id = 2, Name = "Kamran" });

//foreach (var item in AppDbContext<Group>.datas)
//{
//    Console.WriteLine(item.Id + " " + item.Name);
//}

GroupController groupController = new GroupController();

while (true)
{
    ShowMenu();
Operation: string operationStr = Console.ReadLine();

    int operation;

    bool isCorrectOperationFormat = int.TryParse(operationStr, out operation);

    if (isCorrectOperationFormat)
    {
        switch (operation)
        {
            case (int)OperationType.CreateGroup:
                groupController.Create();
                break;

            case (int)OperationType.UpdateGroup:
                groupController.GetAll();
                groupController.Update();
                break;

            case (int)OperationType.DeleteGroup:
                groupController.GetAll();
                groupController.Delete();
                break;

            case (int)OperationType.GetAllGroups:
                groupController.GetAll();
                break;

            case (int)OperationType.GetAllGroupsByTeacher:
                groupController.GetAllByTeacher();
                break;
            case (int)OperationType.GetAllGroupsByRoom:
                groupController.GetAllByRoom();
                break;

            case (int)OperationType.GetGroupById:
                groupController.GetById();
                break;

            case (int)OperationType.SearchGroupsByName:
                groupController.SearchByName();
                break;

            default:
                ConsoleColor.Red.WriteConsole("Operation is wrong, please choose again");
                goto Operation;
        }
    }
}
void ShowMenu()
{
    ConsoleColor.Cyan.WriteConsole("Select one operation:\n" +
        "\n" +
        "1. Create group\n" +
        "2. Update group\n" +
        "3. Delete group\n" +
        "4. Show all groups\n" +
        "5. Show all groups by teacher\n" +
        "6. Show all groups by room\n" +
        "7. Show group by id\n" +
        "8. Search groups by name");
}


