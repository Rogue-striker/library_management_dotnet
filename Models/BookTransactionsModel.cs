public class BookTransactionsModel{
    public int transactionId {get;set;}
    public int userId {get;set;}
    public int bookId{get;set;}

    public string bookName {get;set;}
    public DateTime expiryDate{get;set;}

    public BookTransactionsModel(int transactionId, int userId, int bookId, string bookName , DateTime expiryDate)
    {
        this.transactionId = transactionId;
        this.userId = userId;
        this.bookId = bookId;
        this.expiryDate = expiryDate;
        this.bookName = bookName;
    }
    public BookTransactionsModel(int transactionId, int bookId, string bookName , DateTime expiryDate)
    {
        this.transactionId = transactionId;
        this.bookId = bookId;
        this.expiryDate = expiryDate;
        this.bookName = bookName;
    }
} 