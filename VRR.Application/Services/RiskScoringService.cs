using System;
using System.Collections.Generic;
using System.Text;
using VRR.Application.Interfaces;
using VRR.Domain.Entities;
using VRR.Domain.Enums;

namespace VRR.Application.Services
{
    public class RiskScoringService : IRiskScoringService
    {
        public int CalculateRiskScore(Resident resident)
        {
            int score = 0;

            // Age factor
            int age = CalculateAge(resident.DateOfBirth);
            if (age >= 85) score += 30;
            else if (age >= 75) score += 20;
            else if (age >= 65) score += 10;

            // Mobility factor
            score += resident.MobilityStatus switch
            {
                MobilityStatus.Bedridden => 30,
                MobilityStatus.WheelchairUser => 20,
                MobilityStatus.UsesWalkingAid => 10,
                MobilityStatus.Ambulatory => 0,
                _ => 0
            };

            // Living situation
            if (resident.LivesAlone) score += 15;

            // Medical needs (any text entered counts as a risk factor)
            if (!string.IsNullOrWhiteSpace(resident.MedicalNeeds)) score += 15;

            // Environmental resilience
            if (!resident.HasAirConditioning) score += 10;
            if (!resident.HasBackupPower) score += 10;

            return score;
        }

        private static int CalculateAge(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return age;
        }
    }
}
