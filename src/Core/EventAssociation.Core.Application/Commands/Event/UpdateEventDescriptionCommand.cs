using EventAssociation.Core.Domain.Common.Values.Event;
using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event
{
    public class UpdateEventDescriptionCommand
    {
        public EventId id { get; }
        public EventDescription newDescription { get; }

        private UpdateEventDescriptionCommand(EventId id, EventDescription newDescription)
        {
            this.id = id;
            this.newDescription = newDescription;
        }

        public static Results<UpdateEventDescriptionCommand> Create(string id, string newDescription)
        {
            Results<EventId> idResult = EventId.FromString(id);
            Results<EventDescription> descriptionResult = EventDescription.Create(newDescription);

            List<Error> errors = new List<Error>();

            if (idResult.IsFailure)
            {
                errors.AddRange(idResult.Errors);
            }
            if (descriptionResult.IsFailure)
            {
                errors.AddRange(descriptionResult.Errors);
            }
            if (errors.Any())
            {
                return Results<UpdateEventDescriptionCommand>.Failure(errors.ToArray());
            }
            return Results<UpdateEventDescriptionCommand>.Success(new UpdateEventDescriptionCommand(idResult.Value, descriptionResult.Value));
        }
    }
}
