using System;

namespace Domain.Commands
{
    public class CreateTaskCommand
    {
        public string Subject { get; set; }
        public Guid? AssignedMemberId { get; set; }
    }
}
