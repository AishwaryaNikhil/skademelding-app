namespace TelenorSkademeldingApp.Application.Common.Exceptions;

/// <summary>Thrown when incoming request data fails business validation.</summary>
public sealed class ClaimValidationException : Exception
{
    public ClaimValidationException(string message) : base(message)
    {
    }
}
