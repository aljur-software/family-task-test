using System;

namespace Domain.Commands
{
    public class AssingTaskCommand
    {
        public Guid Id { get; set; }
        public Guid AssignedMemberId { get; set; }
    }
}
