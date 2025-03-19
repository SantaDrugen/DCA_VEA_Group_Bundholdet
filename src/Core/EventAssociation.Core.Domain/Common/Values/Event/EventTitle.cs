using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Domain.Common.Values.Event
{
    internal class EventTitle
    {
        private const int MinLength = 3;
        private const int MaxLength = 75;
        
        public string Title { get; }

        // This should not be public, it needs to change once Event is properly set up.
        public EventTitle(string title)
        {
            Title = title;
        }

        // We check the Title.Length here, to ensure compliance with naming convention outline in Use Case 2.
        public static Results<EventTitle> Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Results<EventTitle>.Failure(new Error("TITLE_EMPTY",
                    "The title must be between 3 and 75 characters."));
            }

            if (title.Length < MinLength || title.Length > MaxLength)
            {
                return Results<EventTitle>.Failure(new Error("TITLE_INVALID_LENGTH",
                    "The title must be between 3 and 75 characters."));
            }
            
            return Results<EventTitle>.Success(new EventTitle(title));
        }
    }
}
