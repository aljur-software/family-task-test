using AutoMapper;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.Queries;
using Domain.ViewModel;
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
            var task = _mapper.Map<Domain.DataModels.Task>(command);
            var persistedTask = await _taskRepository.CreateRecordAsync(task);

            var vm = _mapper.Map<TaskVm>(persistedTask);

            return new CreateTaskCommandResult()
            {
                Payload = vm
            };
        }

        public async Task<AssignTaskCommandResult> AssignTaskCommandHandler(AssignTaskCommand command)
        {
            var task = await _taskRepository.ByIdAsync(command.Id);

            _mapper.Map<AssignTaskCommand, Domain.DataModels.Task>(command, task);

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task); 

            var succeed = affectedRecordsCount == 1;

            return new AssignTaskCommandResult()
            {
                Succeed = succeed
            };
        }

        public async Task<CompleteTaskCommandResult> CompleteTaskCommandHandler(CompleteTaskCommand command)
        {
            var task = await _taskRepository.ByIdAsync(command.Id);

            _mapper.Map<CompleteTaskCommand, Domain.DataModels.Task>(command, task);

            if (!task.IsComplete)
            {
                var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(task);
                if (affectedRecordsCount < 1)
                    return new CompleteTaskCommandResult()
                    {
                        Succeed = false
                    };
            } 

            return new CompleteTaskCommandResult()
            {
                Succeed = true
            };
        }

        public async Task<GetAllTasksQueryResult> GetAllTasksQueryHandler()
        {
            IEnumerable<TaskVm> vm = new List<TaskVm>();

            var tasks = await _taskRepository.Reset().GetAllTasksWithMemberAsync();

            if (tasks != null && tasks.Any())
                vm = _mapper.Map<IEnumerable<TaskVm>>(tasks);

            return new GetAllTasksQueryResult()
            {
                Payload = vm
            };
        }
    }
}
