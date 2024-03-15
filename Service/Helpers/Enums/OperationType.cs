namespace Service.Helpers.Enums
{
    public enum OperationType
    {
        Exit,
        CreateGroup,
        UpdateGroup, 
        DeleteGroup,
        GetAllGroups,
        GetAllGroupsByTeacher,
        GetAllGroupsByRoom, 
        GetGroupById,
        SearchGroupsByName,
        CreateStudent, 
        UpdateStudent,
        DeleteStudent,
        GetAllStudents, 
        GetAllStudentsByAge, 
        GetAllStudentsByGroupId, 
        GetStudentById,
        SearchStudentsByNameOrSurname
    }
}
