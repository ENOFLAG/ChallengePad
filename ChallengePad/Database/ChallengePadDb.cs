using ChallengePad.Channel;
using ChallengePad.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChallengePad.Database
{
    public class ChallengePadDb : IChallengePadDb
    {
        private readonly ILogger Logger;
        private readonly ChallengePadDbContext Context;

        public ChallengePadDb(ChallengePadDbContext context, ILogger<ChallengePadDb> logger)
        {
            Context = context;
            Logger = logger;
        }

        public async Task Migrate()
        {
            await Context.Database.MigrateAsync();
        }

        public async Task CreateOperation(string name, CancellationToken token)
        {
            Logger.LogDebug($"CreateOperation({name})");
            var newOp = new Operation()
            {
                Name = name
            };
            Context.Operations.Add(newOp);
            await Context.SaveChangesAsync(token);
            await OperationsChannel.Publish(newOp.Id);
        }

        public async Task<Operation[]> GetOperations(int skip, int take, CancellationToken token)
        {
            Logger.LogDebug($"GetOperations({skip}, {take})");
            return await Context.Operations
                .OrderByDescending(op => op.Id)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync(token);
        }

        public async Task<Operation> GetOperation(long id, CancellationToken token)
        {
            Logger.LogDebug($"GetOperation({id})");
            return await Context.Operations
                .Where(op => op.Id == id)
                .Include(op => op.Files)
                .Include(op => op.Objectives)
                .ThenInclude(obj => obj.Files)
                .SingleAsync(token);
        }

        public async Task CreateObjective(string name, string category, long points, long operationId, CancellationToken token)
        {
            Logger.LogDebug($"CreateObjective({name}, {category}, {points}, {operationId})");
            Context.Objectives
                .Add(new Objective()
                {
                    Name = name,
                    Category = category,
                    Points = points,
                    OperationId = operationId
                });
            await Context.SaveChangesAsync(token);
            await OperationsChannel.Publish(operationId);
        }

        public async Task UpdateObjective(long objectiveId, long operationId, bool solved, CancellationToken token)
        {
            var obj = await Context.Objectives
                .Where(obj => obj.Id == objectiveId)
                .SingleAsync(token);
            obj.Solved = solved;
            await Context.SaveChangesAsync(token);
            await OperationsChannel.Publish(operationId);
        }

        public async Task AddFiles(ICollection<IFormFile> files, long id, bool isObjectiveFile, string username, CancellationToken token)
        {
            long operationId;
            List<UploadedFile> dbFiles;
            if (isObjectiveFile)
            {
                var objective = await Context.Objectives
                    .Where(o => o.Id == id)
                    .Include(o => o.Files)
                    .SingleAsync(token);
                operationId = objective.OperationId;
                dbFiles = objective.Files;
            }
            else
            {
                var operation = await Context.Operations
                    .Where(o => o.Id == id)
                    .Include(o => o.Files)
                    .SingleAsync(token);
                operationId = id;
                dbFiles = operation.Files;
            }

            foreach (var formFile in files)
            {
                var uf = new UploadedFile()
                {
                    Filename = formFile.FileName,
                    Username = username,
                    Timestamp = DateTime.UtcNow
                };
                dbFiles.Add(uf);
                await Context.SaveChangesAsync(token);
                var filePath = $"./Uploads/{uf.Id}";
                using var stream = new FileStream(filePath, FileMode.Create);
                await formFile.CopyToAsync(stream, token);
            }
            await OperationsChannel.Publish(operationId);
        }

        public async Task<string> GetFileName(long id, CancellationToken token)
        {
            return await Context.Files
                .Where(f => f.Id == id)
                .Select(f => f.Filename)
                .SingleAsync(token);
        }
    }
}
