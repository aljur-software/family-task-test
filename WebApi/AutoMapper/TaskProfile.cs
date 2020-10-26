using AutoMapper;
using Domain.Commands;
using Domain.DataModels;
using Domain.ViewModel;

namespace WebApi.AutoMapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskCommand, Task>();
            CreateMap<CompleteTaskCommand, Task>();
            CreateMap<AssignTaskCommand, Task>();
            CreateMap<Task, TaskVm>().ForMember(
                destination => destination.Member,
                options => options.MapFrom(source => source.AssignedMember)
            );
        }
    }
}
