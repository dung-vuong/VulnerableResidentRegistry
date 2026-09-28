using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VRR.Application.Validation;
using VRR.Domain.Enums;

namespace VRR.Application.DTOs
{
    public class ResidentRegistrationDto
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required, StringLength(200)]
        public string AddressLine { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required, RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Enter a valid ZIP code.")]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        public MobilityStatus MobilityStatus { get; set; }

        [StringLength(500)]
        public string? MedicalNeeds { get; set; }

        public bool LivesAlone { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasBackupPower { get; set; }

        [Required, StringLength(100)]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Required, Phone]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string EmergencyContactRelationship { get; set; } = string.Empty;

        [Required]
        [MustBeTrue(ErrorMessage = "You must give consent to register.")]
        public bool ConsentGiven { get; set; }
    }
}
