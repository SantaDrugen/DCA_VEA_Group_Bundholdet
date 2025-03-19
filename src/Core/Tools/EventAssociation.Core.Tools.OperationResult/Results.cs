namespace EventAssociation.Core.Tools.OperationResult;

public class Results
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<Error> Errors { get; } = new();

    protected Results(bool isSuccess, List<Error> errors)
    {
        IsSuccess = isSuccess;
        if (errors != null)
            Errors.AddRange(errors);
    }

    public static Results Success() => new(true, new List<Error>());
    public static Results Failure(params Error[] errors) => new(false, errors.ToList());

    public static Results Combine(params Results[] results)
    {
        var allErrors = results.SelectMany(r => r.Errors).ToList();
        return allErrors.Any() ? Failure(allErrors.ToArray()) : Success();
    }
}

public class Results<T> : Results
{
    public T Value { get; }

    private Results(T value) : base(true, new List<Error>())
    {
        Value = value;
    }

    private Results(List<Error> errors) : base(false, errors)
    {
    }

    public static Results<T> Success(T value) => new(value);
    public static Results<T> Failure(params Error[] errors) => new(errors.ToList());

    public static implicit operator Results<T>(T value) => Success(value);
}

/*
Basic Result:
Start with a basic implementation, with the class containing 
the value and a potential error message.

This implementation will probably not take you far, however.

Consider whether you want two implementations:

1. one with a payload, and one without
or
2. one with a payload, and a “None”/”Unit” class or
3. some other combination..? Maybe you can surprise me.


Multiple Errors:
Your Result class must be able to contain multiple errors. 
Consider how best to collect multiple errors in your Result. 
There are different approaches.


Error Codes:
Your errors must now at least have both an error code/ID and 
an error message. Optionally an error-type-code.


Optional requirements:
1. Static factory methods to easier create success and failure results.
2. Have errors as constants in separate classes (these would go into 
    the Domain, so you may want to hold off on these until later).
3. Helper methods to combine multiple results into one (or collect 
    errors from multiple into one).
4. Implicit operator/conversion.
5. I have a bunch of extra stuff to enable simpler validation of 
    business rules, and “railway oriented programming”, which is 
    concept from functional programming. The point here is just that 
    you can do a lot with this approach to really change your 
    programming style.

You may also find that some of these requirements make more sense as 
you progress in the project, and it is certainly okay to update your 
result along the way.
 */