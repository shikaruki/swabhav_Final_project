using API.Data_Access;
using API.Model;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebAPI.Controllers
{
    [Route("LibraryWebApi/v1/[controller]")]
    [ApiController]
    public class AdminTask : ControllerBase
    {
        private readonly IDataAccess library;
        private readonly IConfiguration configuration;
        public AdminTask(IDataAccess library, IConfiguration configuration )
        {
            this.library = library;
            this.configuration = configuration;

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

        [HttpGet("ReturnBook/{bookId}/{userId}")]
        public IActionResult ReturnBook(string bookId, string userId)
        {
            var result = library.ReturnBook(int.Parse(userId), int.Parse(bookId));
            return Ok(result == true ? "success" : "not returned");

        }
        [HttpGet("ChangeBlockStatus/{status}/{id}")]
        public IActionResult ChangeBlockStatus(int status, int id)
        {
            if (status == 1)
            {
                library.BlockUser(id);
            }
            else
            {
                library.UnblockUser(id);
            }
            return Ok("success");
        }
        [HttpGet("ChangeEnableStatus/{status}/{id}")]
        public IActionResult ChangeEnableStatus(int status, int id)
        {
            if (status == 1)
            {
                library.ActivateUser(id);
            }
            else
            {
                library.DeactivateUser(id);
            }
            return Ok("success");
        }
        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            var categories = library.GetAllCategories();
            var x = categories.GroupBy(c => c.Category).Select(item =>
            {
                return new
                {
                    name = item.Key,
                    children = item.Select(item => new { name = item.SubCategory }).ToList()
                };
            }).ToList();
            return Ok(x);
        }
        [HttpPost("InsertBook")]
        public IActionResult InsertBook(Book book)
        {
            book.Title = book.Title.Trim();
            book.Author = book.Author.Trim();
            book.Category.Category = book.Category.Category.ToLower();
            book.Category.SubCategory = book.Category.SubCategory.ToLower();

            library.InsertNewBook(book);
            return Ok("Inserted");
        }
        [HttpDelete("DeleteBook/{id}")]
        public IActionResult DeleteBook(int id)
        {
            var returnResult = library.DeleteBook(id) ? "success" : "fail";
            return Ok(returnResult);
        }
        [HttpPost("InsertCategory")]
        public IActionResult InsertCategory(BookCategory bookCategory)
        {
            bookCategory.Category = bookCategory.Category.ToLower();
            bookCategory.SubCategory = bookCategory.SubCategory.ToLower();
            library.CreateCategory(bookCategory);
            return Ok("Inserted");
        }
    }
}
