CREATE PROCEDURE [dbo].[spTodos_UpdateTask]		
	@Task nvarchar(50),
	@AssignedTo int,
	@TodoId int
AS
BEGIN
	Update dbo.Todos 
	set Task = @Task
	where 
	Id = @TodoId
		and AssignedTo = @AssignedTo;
END