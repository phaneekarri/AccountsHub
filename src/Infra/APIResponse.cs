namespace Infra;
public class APIResponse<T> : APIResponse
{
   public T? Data { get; private set;} 

   public APIResponse(int status, T? data, string? message = null) : base(status, message)
   {
      Data = data;
   }

}
public class APIResponse
{
   public int Status { get; private set; }
   
   public string? Message {get; private set;}

   public APIResponse(int status, string? message = null)
   {
      Status = status;  Message = message;
   }

}
