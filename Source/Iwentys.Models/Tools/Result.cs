namespace Iwentys.Models.Tools
{
    public static class Result
    {
        public static Result<TValue> From<TValue>(TValue value, string reason = null)
        {
            return new Result<TValue>(value, reason);
        }
    }

    public class Result<TValue>
    {
        public Result(TValue value, string reason)
        {
            Value = value;
            Reason = reason;
        }

        public string Reason { get; set; }
        public TValue Value { get; set; }
    }
}