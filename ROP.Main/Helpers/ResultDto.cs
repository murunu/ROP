namespace Murunu.ROP.Helpers;

public class ResultDto<T>
{
    public T? Value { get; set; }
    public string? Error { get; set; }
    public bool IsError => Error != null;

    public ResultDto()
    {

    }

    public ResultDto(T value)
    {
        Value = value;
    }

    public ResultDto(string error)
    {
        Error = error;
    }
}