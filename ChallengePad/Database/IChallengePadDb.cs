using ChallengePad.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChallengePad.Database
{
    static class IServiceCollectionExtension
    {
        public static IServiceCollection AddChallengePadDb(this IServiceCollection services)
        {
            services
                .AddScoped<IChallengePadDb, ChallengePadDb>()
                .AddLogging()
                .AddDbContextPool<ChallengePadDbContext>(options =>
                {
                    options.UseNpgsql(
                        "Server=localhost;Port=5432;Database=EnoDatabase;User Id=docker;Password=docker;Timeout=15;SslMode=Disable;",
                        pgoptions => pgoptions.EnableRetryOnFailure());
                }, 90);
            return services;
        }
    }

    public interface IChallengePadDb
    {
        Task Migrate();
        Task<Operation[]> GetOperations(int skip, int take, CancellationToken token);
        Task CreateOperation(string name, CancellationToken token);
        Task<Operation> GetOperation(long id, CancellationToken token);
        Task CreateObjective(string name, string category, long points, long operationId, CancellationToken token);
        Task UpdateObjective(long objectiveId, long operationId, bool solved, CancellationToken token);
        Task AddFiles(ICollection<IFormFile> files, long id, bool isObjectiveFile, string username, CancellationToken token);
        Task<string> GetFileName(long id, CancellationToken token);
    }
}
