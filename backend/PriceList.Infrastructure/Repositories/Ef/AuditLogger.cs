using Microsoft.Extensions.Logging;
using PriceList.Core.Abstractions.Repositories;
using PriceList.Core.Entities;
using PriceList.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceList.Infrastructure.Repositories.Ef
{
    public sealed class AuditLogger : IAuditLogger
    {
        private readonly AppDbContext _db;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<AuditLogger> _logger;

        public AuditLogger(AppDbContext db, ICurrentUserService currentUser, ILogger<AuditLogger> logger)
        {
            _db = db;
            _currentUser = currentUser;
            _logger = logger;
        } 

        public async Task LogAsync(AuditLog log, CancellationToken ct = default)
        {
            log.UserId ??= _currentUser.UserId;
            log.UserName ??= _currentUser.UserName;
            log.IpAddress ??= _currentUser.IpAddress;
            log.UserAgent ??= _currentUser.UserAgent;

            _db.AuditLogs.Add(log);
            await _db.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Audit: {ActionType} by {UserId} on {EntityName}({EntityId}) Success={Success}",
                log.ActionType, log.UserId, log.EntityName, log.EntityId, log.Success);
        }
    }
}
