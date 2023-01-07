using FluentValidation.Results;

namespace UI_Museum.Models.Bases
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; } 
        public T? Data { get; set; }
        public string? Message { get; set; }
        public IEnumerable<ValidationFailure>? Errors { get; set; }

    }
}
