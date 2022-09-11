using System.Data.SqlClient;
public interface ITransactionDatabaseService {
    List<BookTransactionsModel> getUserTransactions(int userId);
    List<BookTransactionsModel> getAllTransactions();
}