using Cumulative3N01605244.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative3N01605244.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly SchoolDbContext _dbContext;

        public TeacherAPIController()
        {
            _dbContext = new SchoolDbContext();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeacher(int id, [FromBody] Teacher updatedTeacher)
        {
            using (var connection = _dbContext.AccessDatabase())
            {
                connection.Open();

                // Check if teacher exists
                var checkCommand = new MySqlCommand("SELECT COUNT(*) FROM Teachers WHERE Id = @Id", connection);
                checkCommand.Parameters.AddWithValue("@Id", id);

                var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
                if (!exists)
                {
                    return NotFound("Teacher not found.");
                }

                // Update teacher details
                var updateCommand = new MySqlCommand("UPDATE Teachers SET Name = @Name, HireDate = @HireDate, Salary = @Salary WHERE Id = @Id", connection);
                updateCommand.Parameters.AddWithValue("@Name", updatedTeacher.Name);
                updateCommand.Parameters.AddWithValue("@HireDate", updatedTeacher.HireDate);
                updateCommand.Parameters.AddWithValue("@Salary", updatedTeacher.Salary);
                updateCommand.Parameters.AddWithValue("@Id", id);

                var rowsAffected = updateCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Teacher updated successfully.");
                }

                return BadRequest("Update failed.");
            }
        }
    }
}

