using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TodoLibrary.DataAccess;
using TodoLibrary.Models;

namespace TodoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
    private readonly ITodoData _data;
    private readonly ILogger<TodosController> _logger;

    public TodosController(ITodoData data, ILogger<TodosController> logger)
    {
        _data = data;
        _logger = logger;
    }


    // GET: api/Todos
    [HttpGet]
    public async Task<ActionResult<List<TodoModel>>> Get()
    {
        _logger.LogInformation("GET: api/Todos");
        try
        {
            //name identifier == Sub   mentioned auth class
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _data.GetAllAssigned(int.Parse(userId));
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"GET all call failed");
            return BadRequest();
        }
    }

    // GET api/Todos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoModel>> Get(int todoId)
    {
        _logger.LogInformation("GET: api/Todos/{id}");
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _data.GetOneAssigned(int.Parse(userId), todoId);
            return Ok(result);  
            
        }
        catch(Exception ex) 
        {
                _logger.LogError(ex,"GET by id call failed");
                return BadRequest();
        }

    }

    // POST api/Todos
    [HttpPost]
    public async Task<ActionResult<TodoModel>> Post([FromBody] string taskDescription)
    {
     
        _logger.LogInformation("Post: api/Todos");
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _data.CreateTask(int.Parse(userId), taskDescription);
            return Ok(result);

        }
        catch (Exception ex)
        {

            _logger.LogError(ex,"task creation failed for");
            return BadRequest();
        };  
    }

    // PUT api/Todos/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int todoId, [FromBody] string taskDescription)
    {
        _logger.LogInformation("PUT: api/Todos/id");
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            await _data.UpdateTask(int.Parse(userId), todoId, taskDescription);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"task update failed for {id}", todoId);
            return BadRequest();
        }
        
    }

    // DELETE api/Todos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int todoId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            await _data.DeleteTask(int.Parse(userId), todoId);
            return Ok();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex,"task deletion failed for {id}", todoId);
            return BadRequest();
        }
    }

    // PUT api/Todos/5/Done
    [HttpPut("{id}/Done")]
    public async Task<IActionResult> Done(int todoId)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            await _data.MarkAsDone(int.Parse(userId), todoId);
            return Ok();
        }
        catch (Exception ex)
        {

            _logger.LogError(ex,"couldn't mark task as done for {id}", todoId);
            return BadRequest();
        }
    }
}