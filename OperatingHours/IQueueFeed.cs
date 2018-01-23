namespace OperatingHours
{
    public interface IQueueFeed
    {
        ICalFeed CalFeed { get; }
        string Queue { get; }
        string Team { get; }
        bool? Override { get; }
        void SetOverride(bool? overrideValue);
    }
}