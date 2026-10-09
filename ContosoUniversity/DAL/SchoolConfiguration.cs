// EF6 DbConfiguration (SqlAzureExecutionStrategy) has no direct EF Core equivalent class.
// EF Core execution strategy is configured via DbContextOptionsBuilder in Startup.cs:
//   options.UseSqlServer(connStr, o => o.EnableRetryOnFailure()).UseLazyLoadingProxies();
// This file is intentionally empty; the class is removed.
namespace ContosoUniversity.DAL
{
}
