using API.Data_Access;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebAPI.Controllers
{
    [Route("LibraryWebApi/v1/[controller]")]
    [ApiController]
    public class UserTask : ControllerBase
    {

        private readonly IDataAccess library;
        private readonly IConfiguration configuration;
        public UserTask(IDataAccess library, IConfiguration configuration )
        {
            this.library = library;
            this.configuration = configuration;

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

    }
}
