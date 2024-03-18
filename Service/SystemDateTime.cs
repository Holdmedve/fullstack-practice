public interface IDateTime
{
    DateTime Now { get; }
}

public class SystemDateTime : IDateTime
{
    public DateTime Now
    {
        get { 
            
            return DateTime.Now; 
        }
    }
}