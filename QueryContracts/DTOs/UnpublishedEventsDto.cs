namespace QueryContracts.DTOs
{
    public record UnpublishedEventsDto
    (
        List<SimpleEventDetails> Drafts,
        List<SimpleEventDetails> Ready,
        List<SimpleEventDetails> Cancelled,
        int MaxPageNumberDrafts,
        int MaxPageNumberReady,
        int MaxPageNumberCancelled
    );
}
