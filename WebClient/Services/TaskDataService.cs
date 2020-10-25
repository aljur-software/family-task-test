using Core.Extensions.ModelConversion;
using Domain.Commands;
using Domain.DataModels;
using Domain.Queries;
using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace WebClient.Services
{
    public class TaskDataService: ITaskDataService
    {
        private readonly HttpClient httpClient;

        public TaskDataService(IHttpClientFactory clientFactory)
        {
            httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            Tasks = new List<TaskVm>();
            LoadTasks();
        }

        public IEnumerable<TaskVm> Tasks { get; private set; }
        public TaskVm SelectedTask { get; private set; }
        public TaskVm DragedTask { get; private set; }


        public event EventHandler TasksUpdated;
        public event EventHandler TaskSelected;
        public event EventHandler<string> CreateTaskFailed;
        public event EventHandler<string> CompleteTaskFailed;


        public void SelectTask(Guid id)
        {
            SelectedTask = Tasks.SingleOrDefault(t => t.Id == id);
            TasksUpdated?.Invoke(this, null);
        }
        public void SelectDragedTask(Guid id)
        {
            DragedTask = Tasks.SingleOrDefault(t => t.Id == id);
        }
        private async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {
            return await httpClient.PostJsonAsync<CreateTaskCommandResult>("tasks", command);
        }
        private async Task<CompleteTaskCommandResult> Complete(CompleteTaskCommand command)
        {
            return await httpClient.PutJsonAsync<CompleteTaskCommandResult>("tasks/complete", command);
        }
        private async Task<AssignTaskCommandResult> Assign(AssignTaskCommand command)
        {
            return await httpClient.PutJsonAsync<AssignTaskCommandResult>("tasks/assign", command);
        }
        private async Task<GetAllTasksQueryResult> GetAllTasks()
        {
            return await httpClient.GetJsonAsync<GetAllTasksQueryResult>("tasks");
        }
        private async void LoadTasks()
        {
            var updatedList = (await GetAllTasks()).Payload;
            if (updatedList != null)
            {
                Tasks = updatedList;
                TasksUpdated?.Invoke(this, null);
                return;
            }
            CreateTaskFailed?.Invoke(this, "The creation was successful, but we can no longer get an updated list of tasks from the server.");
        }

        public async Task CreateTask(TaskVm model)
        {
            var result = await Create(model.ToCreateTaskCommand());
            if(result != null)
                LoadTasks();

            CreateTaskFailed?.Invoke(this, "Unable to create record.");
        }
        public async Task AssignTask(TaskVm model)
        {
            var result = await Assign(model.ToAssignTaskCommand());
            if (result != null)
                LoadTasks();

            CreateTaskFailed?.Invoke(this, "Unable to update record.");
        }


        public async Task ToggleTask(Guid id)
        {
            var taskVm = Tasks.Where(_ => _.Id == id).FirstOrDefault();
            if (taskVm == null) 
                throw new ArgumentNullException("Task was not choosen.");
            var result = await Complete(taskVm.ToCompleteTaskCommand());
            if (result != null && result.Succeed)
            {
                foreach (var taskModel in Tasks)
                {
                    if (taskModel.Id == id)
                    {
                        taskModel.IsComplete = !taskModel.IsComplete;
                    }
                }
            }
            else {
                throw new Exception("Unable to complete task.");
            }
            TasksUpdated?.Invoke(this, null);
        }
    }
}