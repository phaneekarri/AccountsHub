namespace UserAuthApi.Exceptions;

public class ValidationException(string Message, Exception? ex = null) : Exception(Message, innerException: ex);
public class ConflictException(string Message, Exception? ex = null) : Exception(Message, innerException: ex);
public class OtpException(string Message, Exception? ex = null) : Exception(Message, innerException: ex);
