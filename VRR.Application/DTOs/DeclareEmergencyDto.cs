using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using VRR.Domain.Enums;

namespace VRR.Application.DTOs;

public class DeclareEmergencyDto
{
    [Required]
    public EmergencyType Type { get; set; }

    [Required, StringLength(300)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Affected ZIP codes (comma-separated)")]
    public string AffectedZone { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Severity { get; set; } = 3;
}