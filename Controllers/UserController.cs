using System.Security.Cryptography;
using AutoMapper;
using chatbot.Data;
using chatbot.Dtos;
using chatbot.Helpers;
using chatbot.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chatbot.Controllers
{
  // [AllowAnonymous] // This attribute will allow anonymous access to the controller, meaning that no authentication is required to access the controller.
  [Authorize] // This attribute will require the authentication to take place before the access to any of the controller is provided
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {
    private readonly DataContextEF _dataContextEF; // private means this variable can only be accessed within this class. readonly means that this variable can only be assigned once either at declaration or in the constructor. 
    private readonly IMapper mapper; // IMapper is an interface that defines a contract for mapping objects from one type to another.

    private readonly AuthHelper _authHelper;
    public UserController(IConfiguration config, AuthHelper authHelper, DataContextEF dataContextEF)
    {
      _dataContextEF = dataContextEF;
      mapper = new Mapper(
        new MapperConfiguration(
          cfg =>
          { cfg.CreateMap<UserToAddDto, User>(); }
        )
      );
      _authHelper = authHelper;
    }

    #region User CRUD
    [HttpGet("GetUsers")] // Read
    public IEnumerable<User> GetUsers()
    {
      return _dataContextEF.Users.ToList();
    }

    [HttpGet("GetUserById/{userId}")] // Read
    public IActionResult GetUserById(int userId)
    {
      User? user = _dataContextEF.Users.Where(u => u.UserId == userId).FirstOrDefault();
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
      if (!_dataContextEF.Users.Any(u => u.Email == userToAddDto.Email))
      {
        _dataContextEF.Users.Add(user);

        if (_dataContextEF.SaveChanges() > 0)
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
      User? userToUpdate = _dataContextEF.Users.Where(u => u.UserId == userId).FirstOrDefault();
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

        if (_dataContextEF.SaveChanges() > 0)
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
      User? userToDelete = _dataContextEF.Users.Where(u => u.UserId == userId).FirstOrDefault();

      if (userToDelete != null)
      {
        _dataContextEF.Users.Remove(userToDelete);
        if (_dataContextEF.SaveChanges() > 0) return Ok($"User with ID: {userId} deleted successfully.");
        else return BadRequest("Failed to delete user.");
      }
      else
      {
        return NotFound($"User with ID: {userId} not found.");
      }
    }

    [AllowAnonymous]
    [HttpPost("Register")] // Register/Create
    public IActionResult Register(UserForRegistrationDto userForRegistrationDto)
    {
      // First check if the passwords match
      if (userForRegistrationDto.Password == userForRegistrationDto.ConfirmPassword)
      {
        // Then check if the user already exists
        if (!_dataContextEF.Users.Any(u => u.Email == userForRegistrationDto.Email))
        {
          // Generate a random salt byte array for password hashing later. It is used later to concatenate with the password before hashing it.
          byte[] passwordSalt = new byte[128 / 8];
          using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
          {
            rng.GetNonZeroBytes(passwordSalt);
          }
          byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistrationDto.Password, passwordSalt);

          User? newUser = new User
          {
            FirstName = userForRegistrationDto.FirstName,
            LastName = userForRegistrationDto.LastName,
            Email = userForRegistrationDto.Email.ToLower(),
            Gender = userForRegistrationDto.Gender,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreatedAt = DateTime.UtcNow
          };

          _dataContextEF.Users.Add(newUser);
          if (_dataContextEF.SaveChanges() > 0)
          {
            return Ok("User registered successfully.");
          }
          else
          {
            return BadRequest("Failed to register user.");
          }
        }
        else
        {
          return BadRequest("User already exists with the same email.");
        }
      }
      else
      {
        return BadRequest("Passwords do not match.");
      }
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDto userForLoginDto)
    {
      // Check if the user exists
      User? user = _dataContextEF.Users.FirstOrDefault(u => u.Email == userForLoginDto.Email.ToLower());
      if (user != null)
      {
        byte[] inputPasswordHash = _authHelper.GetPasswordHash(userForLoginDto.Password, user.PasswordSalt);

        // Check if the password hash matches the stored password hash
        for (int i = 0; i < inputPasswordHash.Length; i++)
        {
          if (inputPasswordHash[i] != user.PasswordHash[i])
          {
            return Unauthorized("Invalid credentials");
          }
        }

        // If the password matches, return the user details (excluding password)
        // Create JWT token using user's ID
        string token = _authHelper.CreateToken(user.UserId);

        // Return token and optionally some user details
        return Ok(new
        {
          Token = token,
          UserId = user.UserId,
          Email = user.Email,
          FirstName = user.FirstName,
          LastName = user.LastName
        });
      }
      else
      {
        return NotFound("User not found.");
      }
    }




    #endregion

  }
}