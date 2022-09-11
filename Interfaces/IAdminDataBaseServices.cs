using System.Data.SqlClient;

public interface IAdminDataBaseServices {
    public bool AddBook(BookModel book);
    public bool DeleteBook(int id);

    public List<BookTransactionsModel> getAllTransactions();
    public List<BookModel> GetAllBooks();
}

