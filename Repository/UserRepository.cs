// using System.Data;
// using System.Data.SqlClient;
// using BlogApplication.Models;

// namespace BlogApplication.Reposotory{
//     public class UserRepository{
//         private SqlConnection connection;
//         public UserRepository(){
//             var serverName = "DESKTOP-UQEVJ76";
//             string databaseName = "BlogApplication";
//             string connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True;TrustServerCertificate=true";
//             connection = new SqlConnection(connectionString);
//         }
//         public bool AddUser(){
//             connection.Open();
//         }
//     }
// }