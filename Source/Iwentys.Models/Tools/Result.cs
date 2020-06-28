namespace Iwentys.Models.Tools
{
    public static class Result
    {
        public static Result<TValue> From<TValue>(TValue value, string reason = null) => new Result<TValue>(value, reason);
    }

    public class Result<TValue>
    {
        public string Reason { get; set; }
        public TValue Value { get; set; }

        public Result(TValue value, string reason)
        {
            Value = value;
            Reason = reason;
        }
    }
}