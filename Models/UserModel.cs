public class UserModel{
    public int userId {get;set;}
    public string userName {get;set;}
    public string password {get;set;}
    public UserModel(string userName,string password){
        this.userName = userName;
        this.password = password;
    }
    public UserModel(){
        
    }
}