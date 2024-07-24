CREATE PROCEDURE [dbo].[spTodos_TaskDone]			
	@AssignedTo int,
	@TodoId int
AS
BEGIN
	Update dbo.Todos 
	set IsDone = 1
	where 
	Id = @TodoId
		and AssignedTo = @AssignedTo;
END