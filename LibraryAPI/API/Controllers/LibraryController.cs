﻿using API.Data_Access;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using LibraryWebAPI.Services;
 

namespace API.Controllers
{
    [Route("LibraryWebApi/v1/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly IDataAccess library;
        private readonly IConfiguration configuration;

        public LibraryController(IDataAccess library, IConfiguration configuration)
        {
            this.library = library;
            this.configuration = configuration;

        }


        private string GenerateResetToken(string email)
        {
            // Logic to generate a unique reset token for the user
            // You can customize this based on your application's requirements
            // This could involve generating a random token and associating it with the user in your data storage
            return "your-reset-token";
        }



        [HttpPost("CreateAccount")]
        public IActionResult CreateAccount(User user)
        {
            if (!library.IsEmailAvailable(user.Email))
            {
                return Ok("Email is Already Existing !");
            }
            user.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            user.UserType = UserType.USER;
            library.CreateUser(user);
            
            return Ok("Account created successfully!");
        }
        [HttpGet("Login")]
        public IActionResult Login(string email, string password)
        {

            if (library.AuthenticateUser(email, password, out User? user))
            {
                if (user != null)
                {
                    Console.WriteLine(configuration["Jwt:key"]);
                    Console.WriteLine(configuration["Jwt:Duration"].GetType());
                    var jwt = new Jwt(configuration["Jwt:Key"], configuration["Jwt:Duration"]);
                    var token = jwt.GenerateToken(user);
                    return Ok(token);
                }
            }
            return Ok("Invalid");
        }

        [HttpGet("GetAllBooks")]
        public IActionResult GetALlBooks()
        {
            ModelBase model = new ModelBase();
            var books = library.GetAllBooks();
            var booksToSend = books.Select(book => new
            {
                book.Id,
                book.Title,
                book.Category.Category,
                book.Category.SubCategory,
                book.Price,
                Available = !book.Ordered,
                book.Author
            }).ToList();
            return Ok(booksToSend);
        }
      
       

       

    }
}
