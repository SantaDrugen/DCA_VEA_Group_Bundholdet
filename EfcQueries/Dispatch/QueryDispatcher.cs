using QueryContracts.Contracts;

namespace EfcQueries.Dispatch
{
    public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
    {
        public async Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query)
        {
            Type queryInterfaceWithTypes = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TAnswer));
            dynamic handler = serviceProvider.GetService(queryInterfaceWithTypes)!;

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler found for query type {query.GetType().FullName}");
            }

            return await handler.HandleAsync((dynamic)query);
        }
    }
}
