using AutoMapper;
using Domain.Commands;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.AutoMapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskCommand, Task>();
            CreateMap<CompleteTaskCommand, Task>();
            CreateMap<AssignTaskCommand, Task>();
            CreateMap<Task, TaskVm>();
        }
    }
}
