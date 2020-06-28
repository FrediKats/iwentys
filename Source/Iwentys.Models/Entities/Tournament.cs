﻿using System;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TournamentType Type { get; set; }
    }
}