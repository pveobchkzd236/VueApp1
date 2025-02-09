
namespace AiTNMHelper
{
    public static class MsSqlHelper
    {
        public static string ConnectionString { get; set; }
        public static string GetConnectionString()
        {
            return ConnectionString;
        }

        internal static string? ExecuteScalar(string v)
        {
            throw new System.NotImplementedException();

        }
    }
}