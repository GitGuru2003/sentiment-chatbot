using chatbot.Data;
using chatbot.Models;
using Microsoft.AspNetCore.Mvc;

namespace chatbot.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {
    DataContextEF dataContextEF;
    public UserController(IConfiguration config)
    {
      dataContextEF = new DataContextEF(config);
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
      return dataContextEF.Users.ToList();
    }

    [HttpGet("GetUserById/{userId}")]
    public IActionResult GetUserById(int userId)
    {
      User? user = dataContextEF.Users.Where(u => u.UserId == userId).FirstOrDefault();
      if (user != null)
      {
        return Ok(user);
      }
      else
      {
        return NotFound($"User with ID: {userId} not found.");
      }
    }
  }
}