using FluentValidation;

namespace TaskMate.Application.TaskItems.CreateTaskItem
{
    public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
    {
        public CreateTaskItemCommandValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.BoardId).NotEmpty();
        }
    }
}
