﻿namespace Library.API.DTOs.Categories;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record IdContentResult
{
	public int Id { get; set; }
	public string Content { get; set; }
}