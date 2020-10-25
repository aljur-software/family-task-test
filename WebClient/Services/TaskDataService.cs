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


        public event EventHandler TasksUpdated;
        public event EventHandler TaskSelected;
        public event EventHandler<string> CreateTaskFailed;
        public event EventHandler<string> CompleteTaskFailed;


        public void SelectTask(Guid id)
        {
            SelectedTask = Tasks.SingleOrDefault(t => t.Id == id);
            TasksUpdated?.Invoke(this, null);
        }
        private async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {
            return await httpClient.PostJsonAsync<CreateTaskCommandResult>("tasks", command);
        }
        private async Task<CompleteTaskCommandResult> Complete(CompleteTaskCommand command)
        {
            return await httpClient.PutJsonAsync<CompleteTaskCommandResult>("tasks/complete", command);
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

        public async Task ToggleTask(TaskVm model)
        {
            var taskVm = Tasks.Where(_ => _.Id == model?.Id).FirstOrDefault();
            if (taskVm == null) 
                throw new ArgumentNullException("Task was not choosen.");
            var completeCmd = taskVm.ToCompleteTaskCommand();
            completeCmd.IsComplete = !completeCmd.IsComplete;
            var result = await Complete(completeCmd);
            if (result != null && result.Succeed)
            {
                foreach (var taskModel in Tasks)
                {
                    if (taskModel.Id == model?.Id)
                    {
                        taskModel.IsComplete = !taskModel.IsComplete;
                    }
                }
            }
            else {
                model.IsComplete = !model.IsComplete;
                throw new Exception("Unable to complete task.");
            }
            TasksUpdated?.Invoke(this, null);
        }
    }
}