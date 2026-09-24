using System;
using System.Collections.Generic;
using System.Text;
using VRR.Domain.Enums;

namespace VRR.Domain.Entities
{
    public class CheckIn
    {
        public int Id { get; set; }

        public int ResidentId { get; set; }
        public Resident Resident { get; set; } = null!;

        public int EmergencyEventId { get; set; }
        public EmergencyEvent EmergencyEvent { get; set; } = null!;

        public string? AssignedStaffUserId { get; set; }

        public CheckInStatus Status { get; set; } = CheckInStatus.Pending;
        public string? Notes { get; set; }

        public DateTime? AttemptedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
