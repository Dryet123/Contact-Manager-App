using System.Data;
using Microsoft.Data.SqlClient;

namespace ContactManager.Infrastructure.Data
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            
            
            _connectionString = _configuration.GetConnectionString("DefaultConnection") 
                                ?? throw new ArgumentNullException("Connection string for 'DefaultConnection' is not found in appsettings.json.");
        }

        
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}