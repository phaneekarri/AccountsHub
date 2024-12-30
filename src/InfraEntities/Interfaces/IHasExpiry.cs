public interface IHasExpiry
{
    int ExpiryIn {get;}
    bool IsExpired {get;} 
    
}

public interface IHasTimeExpiry : IHasExpiry
{
    ExpiryTimeSpan ExpirySpan {get;}
    DateTime ExpiryAt {get;}
}

public interface IHasDateExpiry : IHasExpiry
{
    ExpiryDateSpan ExpirySpan {get;}
    DateOnly ExpiryAt {get;}
}


public enum ExpiryTimeSpan
{
  Seconds, Minutes, Hours, Days, Months, Years
}

public enum ExpiryDateSpan
{
  Days, Months, Years
}

public class Expiry
{
    public static DateTime Get( DateTime from , int expiresIn, ExpiryTimeSpan span){
        switch(span){
            case ExpiryTimeSpan.Seconds : return from.AddSeconds(expiresIn);
            case ExpiryTimeSpan.Minutes : return from.AddMinutes(expiresIn);
            case ExpiryTimeSpan.Hours : return from.AddHours(expiresIn);
            case ExpiryTimeSpan.Days : return DateOnly.FromDateTime(from).AddDays(expiresIn).ToDateTime(TimeOnly.MinValue);
            case ExpiryTimeSpan.Months : return DateOnly.FromDateTime(from).AddDays(expiresIn).ToDateTime(TimeOnly.MinValue);
            case ExpiryTimeSpan.Years : return DateOnly.FromDateTime(from).AddDays(expiresIn).ToDateTime(TimeOnly.MinValue);
            default:  throw new NotSupportedException("Expiry TimeSpan not supported");
        }
    }

    public static DateOnly Get( DateTime from , int expiresIn, ExpiryDateSpan span){
        return Get(DateOnly.FromDateTime(from), expiresIn, span);
    }

    public static DateOnly Get( DateOnly from , int expiresIn, ExpiryDateSpan span){
        switch(span){
            case ExpiryDateSpan.Days : return from.AddDays(expiresIn);
            case ExpiryDateSpan.Months : return from.AddDays(expiresIn);
            case ExpiryDateSpan.Years : return from.AddDays(expiresIn);
            default:  throw new NotSupportedException("Expiry TimeSpan not supported");
        }
    }

    public static bool IsExpired( DateOnly expiryAt) =>DateOnly.FromDateTime(DateTime.UtcNow) > expiryAt;
    public static bool IsExpired( DateTime expiryAt) =>DateTime.UtcNow > expiryAt;
}