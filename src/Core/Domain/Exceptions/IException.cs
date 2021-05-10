namespace Domain.Exceptions
{
    using System.Collections.Generic;
    public interface IException
    {
        int StatusCode { get; }

        IEnumerable<string> Errors { get; }
    }
}
