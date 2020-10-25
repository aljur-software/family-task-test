using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Repositories
{
    public interface ITaskRepository: IBaseRepository<Guid, Domain.DataModels.Task, ITaskRepository>
    {
        Task<IEnumerable<Domain.DataModels.Task>> GetAllTasksWithMemberAsync(CancellationToken cancellationToken = default);
        Task<int> AssignTaskAsync(Domain.DataModels.Task record, CancellationToken cancellationToken = default);
        Task<int> CompleteTaskAsync(Domain.DataModels.Task record, CancellationToken cancellationToken = default);
    }
}
