using EventAssociation.Core.Tools.OperationResult;

namespace EventAssociation.Core.Application.Commands.Event
{
    public class CreateNewEventCommand
    {

        private CreateNewEventCommand()
        {

        }

        public static Results<CreateNewEventCommand> Create()
        {
            return Results<CreateNewEventCommand>.Success(new CreateNewEventCommand());
        }
    }
}
