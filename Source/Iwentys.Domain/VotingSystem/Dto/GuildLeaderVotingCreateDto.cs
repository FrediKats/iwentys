﻿using System;
using System.Collections.Generic;

namespace Iwentys.Domain.VotingSystem;

public class GuildLeaderVotingCreateDto
{
    public List<int> Candidates { get; set; }
    public DateTime StartTime { get; set; }
}