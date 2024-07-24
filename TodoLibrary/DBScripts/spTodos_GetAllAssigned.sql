CREATE PROCEDURE [dbo].[spTodos_GetAllAssigned]	
	@AssignedTo int
AS
BEGIN
	Select Id, Task, AssignedTo, IsDone from dbo.Todos
	where AssignedTo = @AssignedTo;
END