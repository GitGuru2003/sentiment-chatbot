using AutoMapper;
using chatbot.Data;
using chatbot.Dtos;
using chatbot.Models;
using Microsoft.AspNetCore.Mvc;

namespace chatbot.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {
    DataContextEF dataContextEF;
    IMapper mapper;
    public UserController(IConfiguration config)
    {
      dataContextEF = new DataContextEF(config);
      mapper = new Mapper(
        new MapperConfiguration(
          cfg =>
          { cfg.CreateMap<UserToAddDto, User>(); }
        )
      );

    }

    #region User CRUD
    [HttpGet("GetUsers")] // Read
    public IEnumerable<User> GetUsers()
    {
      return dataContextEF.Users.ToList();
    }

    [HttpGet("GetUserById/{userId}")] // Read
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

    [HttpPost("CreateUser")] // Create
    public IActionResult CreateUser(UserToAddDto userToAddDto)
    {
      User user = mapper.Map<User>(userToAddDto);
      if (!dataContextEF.Users.Any(u => u.Email == userToAddDto.Email))
      {
        dataContextEF.Users.Add(user);

        if (dataContextEF.SaveChanges() > 0)
        {
          return Ok();
        }
        else
        {
          return BadRequest("Failed to create user.");
        }
      }
      else
      {
        return BadRequest("User already exists with the same email.");
      }

    }

    [HttpPut("UpdateUser/{userId}")]
    public IActionResult UpdateUser(int userId, UserToUpdateDto userToUpdateDto)
    {
      User? userToUpdate = dataContextEF.Users.Where(u => u.UserId == userId).FirstOrDefault();
      if (userToUpdate != null)
      {
        // Only update properties that are provided
        if (userToUpdateDto.FirstName != null)
          userToUpdate.FirstName = userToUpdateDto.FirstName;

        if (userToUpdateDto.LastName != null)
          userToUpdate.LastName = userToUpdateDto.LastName;

        if (userToUpdateDto.Email != null)
          userToUpdate.Email = userToUpdateDto.Email.ToLower();

        if (userToUpdateDto.Gender != null)
          userToUpdate.Gender = userToUpdateDto.Gender;

        if (dataContextEF.SaveChanges() > 0)
        {
          return Ok($"User with ID: {userId} updated successfully.");
        }
        else
        {
          return BadRequest("Failed to update the user.");
        }
      }
      else
      {
        return NotFound($"User with ID: {userId} not found.");
      }

    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
      User? userToDelete = dataContextEF.Users.Where(u => u.UserId == userId).FirstOrDefault();

      if (userToDelete != null)
      {
        dataContextEF.Users.Remove(userToDelete);
        if (dataContextEF.SaveChanges() > 0) return Ok($"User with ID: {userId} deleted successfully.");
        else return BadRequest("Failed to delete user.");
      }
      else
      {
        return NotFound($"User with ID: {userId} not found.");
      }
    }



    #endregion

  }
}