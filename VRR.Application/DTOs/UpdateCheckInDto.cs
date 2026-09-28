using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using VRR.Domain.Enums;

namespace VRR.Application.DTOs;

public class UpdateCheckInDto
{
    public int Id { get; set; }

    [Required]
    public CheckInStatus Status { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }
}