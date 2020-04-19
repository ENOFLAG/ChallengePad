﻿using ChallengePad.Models;
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
        public static IServiceCollection AddChallengePadDb(this IServiceCollection services, ChallengePadSettings settings)
        {
            services
                .AddScoped<IChallengePadDb, ChallengePadDb>()
                .AddLogging()
                .AddDbContextPool<ChallengePadDbContext>(options =>
                {
                    options.UseNpgsql(settings.DbConnectionString,
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
        Task<Operation> GetOperation(string name, CancellationToken token);
        Task CreateObjective(string name, string category, long points, long operationId, CancellationToken token);
        Task UpdateObjective(long objectiveId, long operationId, bool solved, CancellationToken token);
        Task UpdateObjectiveVisiblity(long objectiveId, long operationId, bool visible, CancellationToken token);
        Task AddFiles(ICollection<IFormFile> files, long id, bool isObjectiveFile, string username, CancellationToken token);
        Task<string> GetFileName(long id, CancellationToken token);
    }
}
