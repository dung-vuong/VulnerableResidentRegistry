using System;
using System.Collections.Generic;
using System.Text;
using VRR.Application.DTOs;

namespace VRR.Application.Interfaces;

public interface IEmergencyDeclarationService
{
    /// <summary>Declares the emergency and returns the new event's Id.</summary>
    Task<int> DeclareAsync(DeclareEmergencyDto dto, string declaredByUserId);
}