namespace UserAuthApi.Exceptions;

public class ValidationException(string Message) : Exception(Message);
public class ConflictException(string Message) : Exception(Message);
public class OtpException(string Message) : Exception(Message);
