CREATE PROCEDURE [dbo].[spTodos_GetOneAssigned]	
	@AssignedTo int,
	@TodoId int
AS
BEGIN
	Select Id, Task, AssignedTo, IsDone from dbo.Todos
	where AssignedTo = @AssignedTo 
		and Id = @TodoId;
END