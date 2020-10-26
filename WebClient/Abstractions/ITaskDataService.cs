using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Shared.Models;

namespace WebClient.Abstractions
{
    public interface ITaskDataService
    {
        IEnumerable<TaskVm> Tasks { get; }
        TaskVm SelectedTask { get; }
        TaskVm DragedTask { get; }

        event EventHandler TasksUpdated;
        event EventHandler TaskSelected;
        event EventHandler<string> CreateTaskFailed;
        event EventHandler<string> CompleteTaskFailed;

        Task CreateTask(TaskVm model);
        Task AssignTask(TaskVm model);
        Task ToggleTask(Guid id);

        void SelectTask(Guid id);
        void SelectDragedTask(Guid id);
        
    }
}