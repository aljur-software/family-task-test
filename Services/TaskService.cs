using AutoMapper;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain.Commands;
using ddm = Domain.DataModels;
using Domain.Queries;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(IMapper mapper, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }

        public async Task<CreateTaskCommandResult> CreateTaskCommandHandler(CreateTaskCommand command)
        {
            var task = _mapper.Map<ddm.Task>(command);
            var persistedMember = await _taskRepository.CreateRecordAsync(task);

            var vm = _mapper.Map<TaskVm>(persistedMember);

            return new CreateTaskCommandResult()
            {
                Payload = vm
            };
        }

        public async Task<AssignTaskCommandResult> AssignTaskCommandHandler(AssignTaskCommand command)
        {
            var isSucceed = true;
            var task = await _taskRepository.ByIdAsync(command.Id);

            _mapper.Map<AssignTaskCommand, ddm.Task>(command, task);

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task); //create assing method? 

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new AssignTaskCommandResult()
            {
                Succeed = isSucceed
            };
        }

        public async Task<CompleteTaskCommandResult> CompleteTaskCommandHandler(CompleteTaskCommand command)
        {
            var isSucceed = true;
            var task = await _taskRepository.ByIdAsync(command.Id);

            _mapper.Map<CompleteTaskCommand, ddm.Task>(command, task);

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task); //create complete method? 

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new CompleteTaskCommandResult()
            {
                Succeed = isSucceed
            };
        }

        public async Task<GetAllTasksQueryResult> GetAllTasksQueryHandler()
        {
            IEnumerable<TaskVm> vm = new List<TaskVm>();

            var tasks = await _taskRepository.Reset().ToListAsync();

            if (tasks != null && tasks.Any())
                vm = _mapper.Map<IEnumerable<TaskVm>>(tasks);

            return new GetAllTasksQueryResult()
            {
                Payload = vm
            };
        }
    }
}
