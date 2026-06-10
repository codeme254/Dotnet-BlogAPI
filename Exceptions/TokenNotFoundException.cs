namespace BlogAPI.Exceptions;

public class TokenNotFoundException(string message) : Exception(message) { }