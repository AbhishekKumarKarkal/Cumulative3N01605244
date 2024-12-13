using Cumulative3N01605244.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative3N01605244.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly SchoolDbContext _dbContext;

        public TeacherPageController()
        {
            _dbContext = new SchoolDbContext();
        }

        public IActionResult Edit(int id)
        {
            using (var connection = _dbContext.AccessDatabase())
            {
                connection.Open();

                var command = new MySqlCommand("SELECT * FROM Teachers WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var teacher = new Teacher
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        HireDate = Convert.ToDateTime(reader["HireDate"]),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    };
                    return View(teacher);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Teacher teacher)
        {
            using (var connection = _dbContext.AccessDatabase())
            {
                connection.Open();

                var updateCommand = new MySqlCommand("UPDATE Teachers SET Name = @Name, HireDate = @HireDate, Salary = @Salary WHERE Id = @Id", connection);
                updateCommand.Parameters.AddWithValue("@Name", teacher.Name);
                updateCommand.Parameters.AddWithValue("@HireDate", teacher.HireDate);
                updateCommand.Parameters.AddWithValue("@Salary", teacher.Salary);
                updateCommand.Parameters.AddWithValue("@Id", teacher.Id);

                var rowsAffected = updateCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(teacher);
        }
    }
}
