using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using ToDoMysqlApi.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection.PortableExecutable;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Http;

namespace ToDoMysqlApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IConfiguration Configuration;
      
        public MySqlConnection connection, queryConnection;

        public TodoController(IConfiguration configuration)
        {
            Configuration = configuration;
            connection = new MySqlConnection(Configuration["ConnectionStrings:Default"]);
            queryConnection = new MySqlConnection(Configuration["ConnectionStrings:Default"]);
        }

        [HttpGet]
        [Route("getTodos")]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                await queryConnection.OpenAsync();
                await connection.OpenAsync();
                using var queryCommand = new MySqlCommand("SELECT Query FROM Queries WHERE QueryName='GET_ALL_TODOS';", queryConnection);
                await using var queryReader = await queryCommand.ExecuteReaderAsync();
                await queryReader.ReadAsync();

                MySqlCommand command = new MySqlCommand(queryReader.GetString("Query"), connection);
                await using var reader = await command.ExecuteReaderAsync();
                List<Todo> todoList = new() { };

                while (await reader.ReadAsync())
                {
                    todoList.Add(new Todo
                    {
                        Id = reader.GetInt32("Id"),
                        AuthorId = reader.GetInt32("AuthorId"),
                        Content = reader.GetString("Content").ToString(),
                        CreatedDate = reader.GetDateTime("CreatedDate"),
                        IsComplete = reader.GetBoolean("IsComplete")
                    });
                }
                

                return Ok(todoList);
            }
            catch(Exception ex)
            {   
                return BadRequest(new Error
                {
                    ErrorCode = 500,
                    Message = ex.Message,
                });
            }
            finally
            {
                await queryConnection.CloseAsync();
                await connection.CloseAsync();
            }
        }

        [HttpDelete]
        [Route("deleteTodo")]
        public async Task<IActionResult> DeleteTodo(int Id)
        {
            try
            {
                await queryConnection.OpenAsync();
                await connection.OpenAsync();
                using var queryCommand = new MySqlCommand("SELECT Query FROM Queries WHERE QueryName='DELETE_TODO';", queryConnection);
                await using var queryReader = await queryCommand.ExecuteReaderAsync();
                await queryReader.ReadAsync();

                MySqlCommand command = new MySqlCommand(String.Format(queryReader.GetString("Query"), Id), connection);
                await using var reader = await command.ExecuteReaderAsync();
                List<Todo> todoList = new() { };

                while (await reader.ReadAsync())
                {
                    todoList.Add(new Todo
                    {
                        Id = reader.GetInt32("Id"),
                        AuthorId = reader.GetInt32("AuthorId"),
                        Content = reader.GetString("Content").ToString(),
                        CreatedDate = reader.GetDateTime("CreatedDate"),
                        IsComplete = reader.GetBoolean("IsComplete")
                    });
                }


                return Ok(todoList);
            }
            catch (Exception ex)
            {
                return BadRequest(new Error
                {
                    ErrorCode = 500,
                    Message = ex.Message,
                });
            }
            finally
            {
                await queryConnection.CloseAsync();
                await connection.CloseAsync();
            }
        }

        [HttpPost]
        [Route("insertTodo")]
        public async Task<IActionResult> InsertTodo(int AuthorId, string Content)
        {
            try
            {
                await queryConnection.OpenAsync();
                await connection.OpenAsync();
                using var queryCommand = new MySqlCommand("SELECT Query FROM Queries WHERE QueryName='INSERT_TODO';", queryConnection);
                await using var queryReader = await queryCommand.ExecuteReaderAsync();
                await queryReader.ReadAsync();

                MySqlCommand command = new MySqlCommand(String.Format(queryReader.GetString("Query"), AuthorId, Content), connection);
                await using var reader = await command.ExecuteReaderAsync();
                List<Todo> todoList = new() { };

                while (await reader.ReadAsync())
                {
                    todoList.Add(new Todo
                    {
                        Id = reader.GetInt32("Id"),
                        AuthorId = reader.GetInt32("AuthorId"),
                        Content = reader.GetString("Content").ToString(),
                        CreatedDate = reader.GetDateTime("CreatedDate"),
                        IsComplete = reader.GetBoolean("IsComplete")
                    });
                }


                return Ok(todoList);
            }
            catch (Exception ex)
            {
                return BadRequest(new Error
                {
                    ErrorCode = 500,
                    Message = ex.Message,
                });
            }
            finally
            {
                await queryConnection.CloseAsync();
                await connection.CloseAsync();
            }
        }
    }
}