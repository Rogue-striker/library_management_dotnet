using System.Data.SqlClient;

public class DatabaseService : IDatabaseService{
    public SqlConnection con {get;set;}
    public SqlConnection getConnection(){
       // this.con = new SqlConnection("Server=127.0.0.1,1433;Database=library;User Id=SA;Password=Kiran@123!;");
       //this.con = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=librarydb;Trusted_Connection=true");
       this.con = new SqlConnection ("Server=tcp:databaselibrary.database.windows.net,1433;Initial Catalog=libraryDB;Persist Security Info=False;User ID=kiranperaka;Password=Kiran@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        return con;
    }

}
