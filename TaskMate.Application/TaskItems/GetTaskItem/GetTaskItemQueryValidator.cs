using FluentValidation;

namespace TaskMate.Application.TaskItems.GetTaskItem
{
    public class GetTaskItemQueryValidator : AbstractValidator<GetTaskItemQuery>
    {
        public GetTaskItemQueryValidator()
        {
            RuleFor(x=>x.Id).NotEmpty();
        }
    }
}
