using API.Data_Access;
using API.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly IDataAccess library;
        private readonly IConfiguration configuration;
        public LibraryController(IDataAccess library, IConfiguration configuration = null)
        {
            this.library = library;
            this.configuration = configuration;
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
                    Console.WriteLine(configuration["Jwt:Duration"].GetType() );
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
        [HttpGet("OrderBook/{userId}/{bookId}")]
        public IActionResult OrderBook(int userId, int bookId)
        {
            var result = library.OrderBook(userId, bookId) ? "success" : "fail";
            return Ok(result);
        }

        [HttpGet("GetOrders/{id}")]
        public IActionResult GetOrders(int id)
        {
            return Ok(library.GetOrdersOfUser(id));
        }
        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            return Ok(library.GetAllOrders());
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            //call the getUsers func and store it in user and make a dynamic object to get data from that backend 
            var users = library.GetUsers();
            var result = users.Select(user => new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Mobile,
                user.Blocked,
                user.Active,
                user.CreatedOn,
                user.UserType,
                user.Fine
            });
            return Ok(result);
        }

    }
}
