using ddm = Domain.DataModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Repositories
{
    public interface ITaskRepository: IBaseRepository<Guid, ddm.Task, ITaskRepository>
    {
        Task<int> AssignTaskAsync(ddm.Task record, CancellationToken cancellationToken = default);
        Task<int> CompleteTaskAsync(ddm.Task record, CancellationToken cancellationToken = default);
    }
}
