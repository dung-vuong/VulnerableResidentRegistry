using System;
using System.Collections.Generic;
using System.Text;
using VRR.Domain.Entities;

namespace VRR.Application.Interfaces
{
    public interface IRiskScoringService
    {
        int CalculateRiskScore(Resident resident);

    }
}
