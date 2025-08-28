using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace PriceList.Api.Helpers
{
    public static class SqlExceptionHelpers
    {
        public static bool IsUniqueViolation(DbUpdateException ex)
        {
            if (ex.InnerException is Microsoft.Data.SqlClient.SqlException sqlEx)
                return sqlEx.Number is 2601 or 2627;
            return false;
        }
        public static bool IsForeignKeyViolation(DbUpdateException ex) =>
            ex.InnerException is SqlException sqlEx &&
            (sqlEx.Number == 547);
    }
}
