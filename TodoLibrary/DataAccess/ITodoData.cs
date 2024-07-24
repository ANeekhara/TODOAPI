using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public interface ITodoData
{
    Task<List<TodoModel>> GetAllAssigned(int assignedTo);
    Task UpdateTask(int assignedTo, int todoId, string task);
    Task<TodoModel?> CreateTask(int assignedTo, string task);
    Task DeleteTask(int assignedTo, int todoId);
    Task<TodoModel?> GetOneAssigned(int assignedTo, int todoId);
    Task MarkAsDone(int assignedTo, int todoId);
}