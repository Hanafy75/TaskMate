using FluentValidation;

namespace TaskMate.Application.Projects.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x=>x.Id)
                .NotEmpty();

            RuleFor(x=>x.Name)
                .NotEmpty();
        }
    }
}
