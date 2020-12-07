using System;

public class TooManyRequestsException : Exception
{
    // Thrown when the api key used passed its requests limit
    public TooManyRequestsException()
    {
    }

    public TooManyRequestsException(string message)
        : base(message)
    {
    }

    public TooManyRequestsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}