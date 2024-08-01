namespace UserAuthApi.Exceptions;

public class ConflictException(string Message) : Exception(Message);
public class OtpException(string Message) : Exception(Message);
