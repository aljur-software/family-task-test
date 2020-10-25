using Core.Abstractions.Repositories;
using Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ddm = Domain.DataModels;
using System.Threading.Tasks;
using System.Threading;

namespace DataLayer.Repositories
{
    public class TaskRepository : BaseRepository<Guid, ddm.Task, TaskRepository>, ITaskRepository
    {
        public TaskRepository(FamilyTaskContext context) : base(context)
        { }

        public async Task<IEnumerable<ddm.Task>> GetAllTasksWithMemberAsync(CancellationToken cancellationToken = default)
        {
            var result = Query.Include(t => t.AssignedMember).ToList();
            return result;
        }


        ITaskRepository IBaseRepository<Guid, ddm.Task, ITaskRepository>.NoTrack()
        {
            return base.NoTrack();
        }

        ITaskRepository IBaseRepository<Guid, ddm.Task, ITaskRepository>.Reset()
        {
            return base.Reset();
        }

        public async Task<int> AssignTaskAsync(ddm.Task record, CancellationToken cancellationToken = default)
        {
            return await base.UpdateRecordAsync(record, cancellationToken); // what if it was completed by other user?
        }
        public async Task<int> CompleteTaskAsync(ddm.Task record, CancellationToken cancellationToken = default)
        {
            return await base.UpdateRecordAsync(record, cancellationToken); // what if it was assigned by other user?
        }
    }
}
