public class Result
{
    public bool IsSucceeded { get; }

    public bool IsFailed => !IsSucceeded;
    private List<Error> _errors = [];

    public IReadOnlyList<Error> Errors => _errors;

    //constructors
    protected Result()
    {
        IsSucceeded = true;
    }
    protected Result(Error error)
    {
        _errors.Add(error);
    }
    protected Result(List<Error> errors)
    {
        _errors.AddRange(errors);
    }



    //Factory of Methods
    public static Result Ok() => new Result();
    public static Result Fail(Error error) => new Result(error);
    public static Result Fail(List<Error> errors) => new Result(errors);

    ////Implicit Casting Operator 
    public static implicit operator Result(Error error) => Fail(error);
    public static implicit operator Result(List<Error> errors) => Fail(errors);


}



//____________________________

public class Result<TValue> : Result
{
    private TValue _value;
    public TValue Value => IsSucceeded ? _value : throw new InvalidOperationException("Operation Is Not Succeeded");


    private Result(TValue value) : base()
    {
        _value = value;

    }
    private Result(Error error) : base(error)
    {
        _value = default!;
    }
    private Result(List<Error> errors) : base(errors)
    {
        _value = default!;
    }

    //Methods Factory
    public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
    public new static Result<TValue> Fail(Error error) => new Result<TValue>(error);
    public new static Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);

    ////Implicit Casting Operator 
    public static implicit operator Result<TValue>(TValue value) => Ok(value);
    public static implicit operator Result<TValue>(Error error) => Fail(error);
    public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);



}
