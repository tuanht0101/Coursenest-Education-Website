﻿namespace Library.API.DTOs;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record ChangeOrder
{
	public int ToId { get; set; }
	public bool IsBefore { get; set; }
}
