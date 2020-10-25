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
                dest => dest.Member,
                opt => opt.MapFrom(src => src.AssignedMember)
            );
        }
    }
}
