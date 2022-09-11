using dotnet_assignment.Models;
using System.Data.SqlClient;

public interface ILibraryDataBaseService{
    bool lendBook(UserLendBookModel credentials);
    void returnBook (int bookTransactionId);
    List<BookModel> showBooks();
    List<BookTransactionsModel> getUserTransactions(int userId);
}