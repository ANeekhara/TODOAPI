CREATE PROCEDURE [dbo].[spTodos_Create]		
	@Task nvarchar(50),
	@AssignedTo int	
AS
BEGIN
	Insert into dbo.Todos (Task, AssignedTo)
	values (@Task, @AssignedTo);

	Select Id, task, AssignedTo, IsDone
	from dbo.Todos
	where  Id = SCOPE_IDENTITY();
END