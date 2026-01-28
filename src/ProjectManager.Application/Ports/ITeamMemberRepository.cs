using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManager.Application.Ports
{
    public interface ITeamMemberRepository
    {
        Task<bool> IsUserInTeamAsync(Guid userId, Guid teamId, CancellationToken ct = default);
        Task AddMemberAsync(TeamMember member, CancellationToken ct = default);
        Task RemoveMemberAsync(Guid userId, Guid teamId, CancellationToken ct = default);
    }
}
