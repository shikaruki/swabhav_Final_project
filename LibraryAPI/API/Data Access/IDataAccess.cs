using API.Model;

namespace API.Data_Access
{
    public interface IDataAccess
    {
        int CreateUser(User user);
        bool IsEmailAvailable(string email);
        bool AuthenticateUser(string email, string password, out User? user);
        IList<Book> GetAllBooks();
<<<<<<< HEAD

        IList<User>GetUsers();

=======
        bool OrderBook(int userId, int bookId);
        IList<Order> GetOrdersOfUser(int userId);
        IList<Order> GetAllOrders();
>>>>>>> 48111468f37f78cd6ab85a560ac2c272c5171617
    }
}
