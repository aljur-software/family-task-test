using AutoMapper;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.Queries;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var task = _mapper.Map<Domain.DataModels.Task>(command);
            var persistedTask = await _taskRepository.CreateRecordAsync(task);

            var taskVm = _mapper.Map<TaskVm>(persistedTask);

            return new CreateTaskCommandResult
            {
                Payload = taskVm
            };
        }

        public async Task<AssignTaskCommandResult> AssignTaskCommandHandler(AssignTaskCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var task = await _taskRepository.ByIdAsync(command.Id);

            _mapper.Map(command, task);

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task);

            var succeed = affectedRecordsCount == 1;

            return new AssignTaskCommandResult
            {
                Succeed = succeed
            };
        }

        public async Task<CompleteTaskCommandResult> CompleteTaskCommandHandler(CompleteTaskCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var task = await _taskRepository.ByIdAsync(command.Id);

            if (task.IsComplete)
            {
                return new CompleteTaskCommandResult()
                {
                    Succeed = true
                };
            }

            task.IsComplete = true;
            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task);
            var succeed = affectedRecordsCount == 1;
            return new CompleteTaskCommandResult
            {
                Succeed = succeed
            };
        }

        public async Task<GetAllTasksQueryResult> GetAllTasksQueryHandler()
        {
            var tasks = await _taskRepository.Reset().GetAllTasksWithMemberAsync();

            var taskVmList = tasks.Any()
                ? _mapper.Map<List<TaskVm>>(tasks)
                : new List<TaskVm>();

            return new GetAllTasksQueryResult()
            {
                Payload = taskVmList
            };
        }
    }
}
