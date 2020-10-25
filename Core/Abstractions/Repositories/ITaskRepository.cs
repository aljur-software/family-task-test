using ddm = Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Repositories
{
    public interface ITaskRepository: IBaseRepository<Guid, ddm.Task, ITaskRepository>
    {
        Task<IEnumerable<ddm.Task>> GetAllTasksWithMemberAsync(CancellationToken cancellationToken = default);
        Task<int> AssignTaskAsync(ddm.Task record, CancellationToken cancellationToken = default);
        Task<int> CompleteTaskAsync(ddm.Task record, CancellationToken cancellationToken = default);
    }
}
