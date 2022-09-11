using System.Data.SqlClient;

public interface IAuthDatabaseService {
  int Login(UserModel user);
  int Register(UserModel user);
}