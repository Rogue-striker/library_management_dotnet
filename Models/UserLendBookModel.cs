
public class UserLendBookModel{
    public int userId {get;set;}
    public int bookId {get;set;}

    public UserLendBookModel(int userid,int bookid){
        this.userId = userid;
        this.bookId = bookid;
    }
    public UserLendBookModel(){
        
    }

}

