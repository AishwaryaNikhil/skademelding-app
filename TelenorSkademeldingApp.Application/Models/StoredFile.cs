namespace TelenorSkademeldingApp.Application.Models;

/// <summary>Result of persisting a file to storage.</summary>
public sealed record StoredFile(string StoragePath, long SizeBytes);
