using API.Model;

namespace API.Data_Access
{
    public interface IDataAccess
    {
        int CreateUser(User user);
        bool IsEmailAvailable(string email);
        bool AuthenticateUser(string email, string password, out User? user);
        IList<Book> GetAllBooks();

        IList<User>GetUsers();

        bool OrderBook(int userId, int bookId);
        IList<Order> GetOrdersOfUser(int userId);
        IList<Order> GetAllOrders();

        bool ReturnBook(int userId, int bookId);

    }
}
