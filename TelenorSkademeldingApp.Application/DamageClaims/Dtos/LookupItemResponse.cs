namespace TelenorSkademeldingApp.Application.DamageClaims.Dtos;

public sealed record LookupItemResponse(int Id, string Code, string Name, string? Description);
