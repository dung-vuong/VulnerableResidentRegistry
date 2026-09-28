using System;
using System.Collections.Generic;
using System.Text;

using VRR.Application.Services;
using VRR.Domain.Entities;
using VRR.Domain.Enums;
using Xunit;

public class RiskScoringServiceTests
{
    [Fact]
    public void HighRiskResident_ScoresHigh()
    {
        var service = new RiskScoringService();
        var resident = new Resident
        {
            DateOfBirth = new DateOnly(1935, 1, 1), // ~90 years old
            MobilityStatus = MobilityStatus.Bedridden,
            LivesAlone = true,
            MedicalNeeds = "Oxygen-dependent",
            HasAirConditioning = false,
            HasBackupPower = false
        };

        int score = service.CalculateRiskScore(resident);

        Assert.True(score >= 90); // should trip nearly every risk factor
    }
}