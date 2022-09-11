using System.Data.SqlClient;
public class TransactionDatabaseService : ITransactionDatabaseService
{
    
    // private List<BookTransactionsModel> transactions = new List<BookTransactionsModel>();
    // private List<BookTransactionsModel> allTransactionsList = new List<BookTransactionsModel>();

    public IDatabaseService dbService;
    private SqlConnection? con;

    public TransactionDatabaseService(IDatabaseService dbService){
        this.dbService = dbService;
        this.con = dbService.getConnection();
    }


    public List<BookTransactionsModel> getAllTransactions()
    {
        List<BookTransactionsModel> allTransactionsList = new List<BookTransactionsModel>();
        try{
            con.Open();
            string allTransactions = "SELECT BookTransactions.transactionId,BookTransactions.userId,BookTransactions.bookId,BookTransactions.expiryDate,Books.bookName FROM Books,BookTransactions where BookTransactions.bookId = Books.bookId;";
            try{
                SqlCommand allTransactionQuery = new SqlCommand(allTransactions,con);
                SqlDataReader transactionsReader = allTransactionQuery.ExecuteReader();
                while(transactionsReader.Read()){
                    BookTransactionsModel new_transaction = new BookTransactionsModel((int)transactionsReader["transactionId"],(int)transactionsReader["userId"],(int)transactionsReader["bookId"],transactionsReader["bookName"].ToString(),(DateTime)transactionsReader["expiryDate"]);
                    allTransactionsList.Add(new_transaction);
                }
                con.Close();
                return allTransactionsList;
            }
            catch(Exception query_error){
                Console.WriteLine("query error");
            }
            con.Close();
        }
        catch(Exception e){
            Console.WriteLine("connection failed");
        }
        return allTransactionsList;
    }

    public List<BookTransactionsModel> getUserTransactions(int userId)
    {
        List<BookTransactionsModel> transactions = new List<BookTransactionsModel>();

        try
        {
            con.Open();
            string transaction_query = "SELECT BookTransactions.transactionId,BookTransactions.bookId,BookTransactions.expiryDate,Books.bookName FROM Books,BookTransactions where BookTransactions.userId = @userid and BookTransactions.bookId = Books.bookId;";
            try
            {
                SqlCommand historycmd = new SqlCommand(transaction_query, con);
                historycmd.Parameters.AddWithValue("@userid", userId);
                SqlDataReader historyReader = historycmd.ExecuteReader();
                while (historyReader.Read())
                {
                    BookTransactionsModel new_transaction = new BookTransactionsModel((int)historyReader["transactionId"],(int) historyReader["bookId"], historyReader["bookName"].ToString(),(DateTime)historyReader["expiryDate"]);
                    transactions.Add(new_transaction);
                }
                con.Close();
                return transactions;
            }
            catch (Exception e)
            {
                Console.WriteLine("reader error");
                Console.WriteLine(e);
            }
            
            con.Close();

        }
        catch (Exception connectionFailed)
        {
            Console.WriteLine(connectionFailed);
            Console.WriteLine("connection failed");
        }
        return transactions;
    }
}