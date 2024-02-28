
public class StackDataFetchedEvent : GameEvent
{
    public StackDataResponse StackDataResponse { get; private set; }

    public static void Trigger(StackDataResponse stackDataResponse)
    {
        var stackDataFetchedEvent = new StackDataFetchedEvent
        {
            StackDataResponse = stackDataResponse
        };

        GameEvents.Trigger(stackDataFetchedEvent);
    }
}
