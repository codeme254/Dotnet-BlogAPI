namespace BlogAPI.Exceptions;

public class UserNotFoundException(string message) : Exception(message) { }