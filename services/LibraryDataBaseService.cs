using System.Data.SqlClient;
public class LibraryDataBaseService : ILibraryDataBaseService{
    private IDatabaseService service;
    private SqlConnection con;
    public LibraryDataBaseService(IDatabaseService service){
        this.service = service;
        this.con = service.getConnection();
    }
    public LibraryDataBaseService(){}
    public bool lendBook(UserLendBookModel credentials){
        bool decrease = true;
        try{
            con.Open();
            string dt = DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd");
            string insert = "INSERT INTO BookTransactions(userId,bookId,expiryDate) Values(@userid,@bookid,@expiryDate)";
            try{
                SqlCommand inst = new SqlCommand(insert, con);
                inst.Parameters.AddWithValue("@userid", credentials.userId);
                inst.Parameters.AddWithValue("@bookid", credentials.bookId);
                inst.Parameters.AddWithValue("@expiryDate", dt);
                inst.ExecuteNonQuery();
                try{
                    updateCopies(credentials.bookId,decrease);
                    con.Close();
                    return true;
                }
                catch (Exception e){
                     con.Close();
                    Console.WriteLine("update not succesfull"+e.Message);
                    return false;
                }
            }
            catch (Exception e){
                con.Close();
                Console.WriteLine(e.Message);
            }
            con.Close();
        }
        catch (Exception e){
            Console.WriteLine("connection failed" + e.Message);
        }
        return false;
    }


    private void updateCopies(int bookId, bool decrease){
        try{
            string update_query = "";
            if(decrease){
                update_query = "UPDATE BooksAvailable SET copiesAvailable = copiesAvailable -1 WHERE bookId=@bookid";
            }
            else{
                update_query = "UPDATE BooksAvailable SET copiesAvailable = copiesAvailable +1 WHERE bookId=@bookid";
            }
                SqlCommand decreaseCount = new SqlCommand(update_query, con);
                decreaseCount.Parameters.AddWithValue("@bookid", bookId);
                decreaseCount.ExecuteNonQuery();
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
        }
    }

    public void returnBook(int bookTransactionId){
        bool decrease = false;
        try{
            con.Open();
            try{
                int bookid = -1;
                try{
                    SqlCommand cmd = new SqlCommand("SELECT bookId FROM BookTransactions where transactionId=@transactionid;", con);
                    cmd.Parameters.AddWithValue("@transactionid", bookTransactionId);
                    SqlDataReader bookidreader = cmd.ExecuteReader();
                    if (bookidreader.Read()){
                        bookid = (int)bookidreader["bookId"];
                    }
                    bookidreader.Close();
                }
                catch (Exception e){
                    Console.WriteLine("cant get bookid"+e.Message);
                }
                string delquery = "DELETE FROM BookTransactions WHERE transactionId=@transactionid";
                SqlCommand delcmd = new SqlCommand(delquery, con);
                delcmd.Parameters.AddWithValue("@transactionid", bookTransactionId);
                int res = delcmd.ExecuteNonQuery();
                try{
                    updateCopies(bookid,decrease);
                }
                catch (Exception e){
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e){
                Console.WriteLine("deletion failed" + e.Message);
            }
            con.Close();
        }
        catch (Exception e){
            Console.WriteLine("connection failed"+e.Message);
        }
    }

    public List<BookModel> showBooks(){
        List<BookModel> books = new List<BookModel>();
        try{
            con.Open();
            string getAllBooks = "SELECT Book.bookId,Book.bookName,Book.bookAuthor,BooksAvailable.copiesAvailable FROM Book,BooksAvailable WHERE Book.bookId = BooksAvailable.bookId AND BooksAvailable.copiesAvailable > 0 ";
            SqlCommand fetchBooks = new SqlCommand(getAllBooks,con);
            SqlDataReader booksReader = fetchBooks.ExecuteReader();
            while(booksReader.Read()){
                BookModel book = new BookModel((int)booksReader["bookId"],booksReader["bookName"].ToString(),booksReader["bookAuthor"].ToString(),(int)booksReader["copiesAvailable"]);
                books.Add(book);
            }
            con.Close();
        }
        catch(Exception connectionFailed){
            con.Close();
            Console.WriteLine(connectionFailed.Message);
        }
        return books;
    }
     public List<BookTransactionsModel> getUserTransactions(int userId){
        List<BookTransactionsModel> transactions = new List<BookTransactionsModel>();
        try{
            con.Open();
            string transaction_query = "SELECT BookTransactions.transactionId,BookTransactions.bookId,BookTransactions.expiryDate,Book.bookName FROM Book,BookTransactions where BookTransactions.userId = @userid and BookTransactions.bookId = Book.bookId;";
            try{
                SqlCommand historycmd = new SqlCommand(transaction_query, con);
                historycmd.Parameters.AddWithValue("@userid", userId);
                SqlDataReader historyReader = historycmd.ExecuteReader();
                while (historyReader.Read()){
                    BookTransactionsModel new_transaction = new BookTransactionsModel((int)historyReader["transactionId"],(int) historyReader["bookId"], historyReader["bookName"].ToString(),(DateTime)historyReader["expiryDate"]);
                    transactions.Add(new_transaction);
                }
                con.Close();
                return transactions;
            }
            catch (Exception e){
                Console.WriteLine("reader error"+e.Message);
                Console.WriteLine(e);
            }
            con.Close();
        }
        catch (Exception connectionFailed){
            Console.WriteLine("connection failed"+connectionFailed.Message);
        }
        return transactions;
    }
}