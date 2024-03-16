
using CourseApp.Controllers;
using Domain.Models;
using Repository.Data;
using Service.Helpers.Enums;
using Service.Helpers.Extensions;
using Service.Services;

GroupController groupController = new();

StudentController studentController = new();

while (true)
{
Operation: ShowMenu();
    string operationStr = Console.ReadLine();

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
                groupController.Update();
                break;

            case (int)OperationType.DeleteGroup:
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

            case (int)OperationType.CreateStudent:
                studentController.Create();
                break;

            case (int)OperationType.UpdateStudent:
                studentController.Update();
                break;

            case (int)OperationType.DeleteStudent:
                studentController.Delete();
                break;

            case (int)OperationType.GetAllStudents:
                studentController.GetAll();
                break;

            default:
                ConsoleColor.Red.WriteConsole("Operation is wrong, please choose again");
                goto Operation;
        }
    }
}

static void ShowMenu()
{
    ConsoleColor.Cyan.WriteConsole("Select one operation:\n\n" +
        "1. Create group                               9. Create student\n" +
        "2. Update group                              10. Update student\n" +
        "3. Delete group                              11. Delete student\n" +
        "4. Show all groups                           12. Show all students\n" +
        "5. Show all groups by teacher                13. Show all students by age\n" +
        "6. Show all groups by room                   14. Show all students by group id\n" +
        "7. Show group by id                          15. Show student by id\n" +
        "8. Search groups by name                     16. Search students by name or surname\n\n" +
        "0. Exit");
}


