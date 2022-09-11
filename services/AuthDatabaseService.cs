using System.Data.SqlClient;

public class AuthDatabaseService : IAuthDatabaseService{
    public IDatabaseService dbService;
    private SqlConnection con;
    public AuthDatabaseService(IDatabaseService dbService){
        this.dbService = dbService;
        this.con = dbService.getConnection();
    } 
    public int Login(UserModel user){
        try{
            con.Open();
            try{
                string findUserQuery = "SELECT userId from Users WHERE userName = @userName and password=@password";
                SqlCommand findUsercmd = new SqlCommand(findUserQuery,con);
                findUsercmd.Parameters.AddWithValue("@userName",user.userName);
                findUsercmd.Parameters.AddWithValue("@password",user.password);
                SqlDataReader userReader = findUsercmd.ExecuteReader();
                if(userReader.Read()){

                    var userid = Convert.ToInt32(userReader["userId"]);
                    con.Close();
                    return userid;
                }
                else{
                    con.Close();
                    return -1;
                }
            }
            catch(Exception findUserException){
                con.Close();
                 System.Console.WriteLine(findUserException.ToString());
            }
            con.Close();
        }
        catch(Exception connectionFailed){
            Console.WriteLine(connectionFailed.ToString());
        }
        return -1;
    }
    public int Register(UserModel user){
        try{
            con.Open();
            int userId = -1;
            SqlCommand registeruser = new SqlCommand("INSERT INTO Users(userName,password) VALUES(@username,@password) SELECT SCOPE_IDENTITY()",con);
            registeruser.Parameters.AddWithValue("@username",user.userName);
            registeruser.Parameters.AddWithValue("@password",user.password);
            try{
                userId =  Convert.ToInt32(registeruser.ExecuteScalar());
            }
            catch(Exception registerError){
                con.Close();
                Console.WriteLine(registerError.Message);
                throw new Exception();
            }
            con.Close();
            return userId;
        }   
        catch(Exception connectionFailed){
             con.Close();
            Console.WriteLine(connectionFailed.Message);
        }
        return 0;
    }
}