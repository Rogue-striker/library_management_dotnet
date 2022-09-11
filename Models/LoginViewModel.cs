public class LoginViewModel{
    public int userId{get;set;}
    public string password {get;set;}

    public LoginViewModel(int userId, string password)
    {
        this.userId = userId;
        this.password = password;
    }
}