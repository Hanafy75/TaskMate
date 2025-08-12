using FluentValidation;

namespace TaskMate.Application.TaskItems.UpdateTaskITem
{
    public class UpdateTaskITemCommandValidator : AbstractValidator<UpdateTaskITemCommand>
    {
        public UpdateTaskITemCommandValidator()
        {
            RuleFor(x=>x.Id).NotEmpty();
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.Status).NotEmpty();
            RuleFor(x=>x.DueDate).NotEmpty();
        }
    }
}
