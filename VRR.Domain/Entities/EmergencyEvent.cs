using System;
using System.Collections.Generic;
using System.Text;
using VRR.Domain.Enums;

namespace VRR.Domain.Entities
{
    public class EmergencyEvent
    {
        public int Id { get; set; }
        public EmergencyType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public string AffectedZone { get; set; } = string.Empty; // e.g. zip codes, comma-separated for now
        public int Severity { get; set; } // 1-5 scale

        public DateTime DeclaredAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAt { get; set; }
        public string DeclaredByUserId { get; set; } = string.Empty;

        public bool IsActive => EndedAt == null;

        // Navigation
        public ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
    }
}
