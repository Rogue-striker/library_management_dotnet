using System.Data.SqlClient;

public interface IDatabaseService {
    
     SqlConnection getConnection();

}