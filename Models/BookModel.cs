public class BookModel{
    public int bookID{get;set;}
    public string bookName {get;set;}
    public string bookAuthor {get;set;}
    public int noOfCopies {get;set;}

    public BookModel(int bookID, string bookName, string bookAuthor, int noOfCopies)
    {
        this.bookID = bookID;
        this.bookName = bookName;
        this.bookAuthor = bookAuthor;
        this.noOfCopies = noOfCopies;
    }
     public BookModel(string bookName, string bookAuthor, int noOfCopies)
    {
        this.bookName = bookName;
        this.bookAuthor = bookAuthor;
        this.noOfCopies = noOfCopies;
    }
    public BookModel(){}
}