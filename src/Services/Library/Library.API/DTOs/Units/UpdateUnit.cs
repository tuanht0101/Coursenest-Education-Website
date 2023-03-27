namespace Library.API.DTOs.Units;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record UpdateUnit
{
	public string? Title { get; set; }
	public int? RequiredMinutes { get; set; }
}

public record UpdateExam : UpdateUnit
{
}

public record UpdateMaterial : UpdateUnit
{
	public string? Content { get; set; }
}