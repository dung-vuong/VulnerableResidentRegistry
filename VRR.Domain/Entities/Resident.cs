using System;
using System.Collections.Generic;
using System.Text;
using VRR.Domain.Enums;

namespace VRR.Domain.Entities
{
    public class Resident
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }

        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public MobilityStatus MobilityStatus { get; set; }
        public string? MedicalNeeds { get; set; }
        public bool LivesAlone { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasBackupPower { get; set; }

        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
        public string EmergencyContactRelationship { get; set; } = string.Empty;

        public int RiskScore { get; set; }
        public ResidentStatus Status { get; set; } = ResidentStatus.PendingVerification;

        public bool ConsentGiven { get; set; }
        public DateTime? ConsentDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
    }
}
