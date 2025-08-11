using FluentValidation;

namespace TaskMate.Application.Projects.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x=>x.Name)
                .NotEmpty();

        }
    }
}
