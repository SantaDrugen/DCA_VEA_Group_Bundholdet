namespace QueryContracts.Contracts
{
    public interface IQueryDispatcher
    {
        Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query);
    }
}
