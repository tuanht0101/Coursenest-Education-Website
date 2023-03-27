﻿using CommonLibrary.API.Models;

namespace CommonLibrary.API.MessageBus.Responses;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record CourseTierResult
{
	public CourseTier Tier { get; set; }
}
