public class StackHighlightedEvent : GameEvent
{
    public StackTemplate StackTemplate { get; private set; }

    public static void Trigger(StackTemplate stackTemplate)
    {
        var stackHighlightedEvent = new StackHighlightedEvent()
        {
            StackTemplate = stackTemplate
        };

        GameEvents.Trigger(stackHighlightedEvent);
    }
}
