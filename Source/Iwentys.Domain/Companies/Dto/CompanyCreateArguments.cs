﻿namespace Iwentys.Domain.Companies
{
    public class CompanyCreateArguments
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
    }
}