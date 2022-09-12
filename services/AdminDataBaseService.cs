using System.Data.SqlClient;
public class AdminDataBaseService : IAdminDataBaseServices {
    private SqlConnection con;
    public IDatabaseService dbService;
    public AdminDataBaseService(IDatabaseService dbService){
        this.dbService = dbService;
        this.con = dbService.getConnection();
    }
    public bool AddBook(BookModel newBook){

        try{
            con.Open();
            string addBookQuery = "INSERT INTO Book(bookName,bookAuthor,noOfCopies) OUTPUT INSERTED.bookId VALUES (@bookName,@bookAuthor,@noOfCopies)";
            SqlCommand insertBook = new SqlCommand(addBookQuery,con);
            insertBook.Parameters.AddWithValue("@bookName",newBook.bookName);
            insertBook.Parameters.AddWithValue("@bookAuthor",newBook.bookAuthor);
            insertBook.Parameters.AddWithValue("@noOfCopies",newBook.noOfCopies);
            try{
                int bookId = (int) insertBook.ExecuteScalar();
                SqlCommand addCopies = new SqlCommand("INSERT INTO BooksAvailable VALUES(@bookId,@copiesAvailable)",con);
                addCopies.Parameters.AddWithValue("@bookId",bookId);
                addCopies.Parameters.AddWithValue("@copiesAvailable",newBook.noOfCopies);
                try{
                    addCopies.ExecuteNonQuery();
                    con.Close();    
                    return true;
                }
                catch(Exception addCopiesFailed){
                    Console.WriteLine(addCopiesFailed.ToString());
                    return false;
                }
            }
            catch(Exception insertionFailed){
                con.Close();
                Console.WriteLine(insertionFailed.Message);
            }
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
        }
        con.Close();
        return false;
    }

    public bool DeleteBook(int bookId){

        try{
            con.Open();
            string deleteBook = "DELETE FROM BooksAvailable WHERE bookId = @bookid";
            SqlCommand deleteBookCmd = new SqlCommand(deleteBook,con);
            deleteBookCmd.Parameters.AddWithValue("@bookid",bookId);
            try{
                int res = deleteBookCmd.ExecuteNonQuery();
                try{
                    SqlCommand deleteCopies = new SqlCommand("DELETE FROM Book WHERE bookId = @bookId",con);
                    deleteCopies.Parameters.AddWithValue("@bookId",bookId);
                    int confirmation = deleteCopies.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
                catch(Exception deleteCopies){
                    con.Close();
                    Console.WriteLine(deleteCopies.Message);
                }
            }
            catch(Exception deleteExecption){
                con.Close();
                System.Console.WriteLine(deleteExecption.Message);
            }
         
        }
        catch(Exception connectionFailed){
            con.Close();
            Console.WriteLine(connectionFailed.Message);
        }
        return false;
    }

    public List<BookModel> GetAllBooks(){
        List<BookModel> allBooks = new List<BookModel>();
        try{
            con.Open();
            string getAllBooksQuery = "SELECT * FROM Book";
            SqlCommand getAllBookscmd = new SqlCommand(getAllBooksQuery,con);
            try{
                SqlDataReader getAllBooksReader = getAllBookscmd.ExecuteReader();
                while(getAllBooksReader.Read()){
                    BookModel book = new BookModel((int)getAllBooksReader["bookId"],getAllBooksReader["bookName"].ToString(),getAllBooksReader["bookAuthor"].ToString(),(int)getAllBooksReader["noOfCopies"]);
                    allBooks.Add(book);
                }
                  con.Close();
            }
            catch(Exception dataReaderFailed){
                con.Close();
                Console.WriteLine(dataReaderFailed.Message);
            }
        }
        catch(Exception connectionFailed){
            con.Close();
            Console.WriteLine(connectionFailed.Message);
        }
        
        return allBooks;
    }

    public List<BookTransactionsModel> getAllTransactions()
    {
        List<BookTransactionsModel> allTransactionsList = new List<BookTransactionsModel>();
        try{
            con.Open();
            string allTransactions = "SELECT BookTransactions.transactionId,BookTransactions.userId,BookTransactions.bookId,BookTransactions.expiryDate,Book.bookName FROM Book,BookTransactions where BookTransactions.bookId = Book.bookId;";
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
                Console.WriteLine(query_error.Message);
            }
            con.Close();
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
        }
        return allTransactionsList;
    }



}

