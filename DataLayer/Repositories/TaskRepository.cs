using Core.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace DataLayer.Repositories
{
    public class TaskRepository : BaseRepository<Guid, Domain.DataModels.Task, TaskRepository>, ITaskRepository
    {
        public TaskRepository(FamilyTaskContext context) : base(context)
        { }

        public async Task<IEnumerable<Domain.DataModels.Task>> GetAllTasksWithMemberAsync(CancellationToken cancellationToken = default)
        {
            var result = Query.Include(t => t.AssignedMember).ToList();
            return result;
        }


        ITaskRepository IBaseRepository<Guid, Domain.DataModels.Task, ITaskRepository>.NoTrack()
        {
            return base.NoTrack();
        }

        ITaskRepository IBaseRepository<Guid, Domain.DataModels.Task, ITaskRepository>.Reset()
        {
            return base.Reset();
        }

        public async Task<int> AssignTaskAsync(Domain.DataModels.Task record, CancellationToken cancellationToken = default)
        {
            return await base.UpdateRecordAsync(record, cancellationToken); // what if it was completed by other user?
        }
        public async Task<int> CompleteTaskAsync(Domain.DataModels.Task record, CancellationToken cancellationToken = default)
        {
            return await base.UpdateRecordAsync(record, cancellationToken); // what if it was assigned by other user?
        }
    }
}
