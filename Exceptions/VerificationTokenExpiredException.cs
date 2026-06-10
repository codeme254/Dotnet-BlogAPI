namespace BlogAPI.Exceptions;

public class VerificationTokenExpiredException(string message) : Exception(message) { }