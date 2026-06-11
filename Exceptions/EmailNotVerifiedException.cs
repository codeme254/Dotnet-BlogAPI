namespace BlogAPI.Exceptions;

public class EmailNotVerifiedException(string message) : Exception(message) { }