public class BooksAvailableModel {
    public int bookId {get;set;}

    public int copiesAvailable{get;set;}

    public BooksAvailableModel(int bookId, int copiesAvailable)
    {
        this.bookId = bookId;
        this.copiesAvailable = copiesAvailable;
    }
}