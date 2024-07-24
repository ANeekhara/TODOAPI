using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public class TodoData : ITodoData
{
    private const string GETALLASSIGNED = "dbo.spTodos_GetAllAssigned";
    private const string UPDATETASK = "dbo.spTodos_UpdateTask";
    private const string GETONEASSIGEND = "dbo.spTodos_GetOneAssigned";
    private const string MARKTASKDONE = "dbo.spTodos_TaskDone";
    private const string CREATETASK = "dbo.spTodos_Create";
    private const string DELETETASK = "dbo.spTodos_Delete";

    private readonly ISqlDataAccess _sql;

    public TodoData(ISqlDataAccess sql)
    {
        _sql = sql;

    }

    public Task<List<TodoModel>> GetAllAssigned(int assignedTo)
    {
        return _sql.LoadData<TodoModel, dynamic>(GETALLASSIGNED, new {AssignedTo = assignedTo},
            "DBStorageString");
    }

    public async Task<TodoModel?> GetOneAssigned(int assignedTo, int todoId)
    {
        var results = await  _sql.LoadData<TodoModel, dynamic>(GETONEASSIGEND, new {AssignedTo = assignedTo, TodoId = todoId},
            "DBStorageString");
        return results.FirstOrDefault();
    }

    public async Task<TodoModel?> CreateTask(int assignedTo, string task)
    {
        var results =  await _sql.LoadData<TodoModel, dynamic>(CREATETASK, new {AssignedTo = assignedTo, Task= task},
            "DBStorageString");
        return results.FirstOrDefault();
    }

    public Task UpdateTask(int assignedTo, int todoId, string task)
    {
        return _sql.SaveData<dynamic>(UPDATETASK, new {AssignedTo = assignedTo, TodoId = todoId, Task = task},
            "DBStorageString");
    }

    public Task MarkAsDone(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>(MARKTASKDONE, new {AssignedTo = assignedTo, TodoId = todoId},
            "DBStorageString");
    }
    
    public Task DeleteTask(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>(DELETETASK, new {AssignedTo = assignedTo, TodoId = todoId},
            "DBStorageString");
    }
}