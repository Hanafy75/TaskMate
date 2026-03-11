using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Dtos;
using TaskMate.Application.Projects.CreateProject;
using TaskMate.Application.Projects.DeleteProject;
using TaskMate.Application.Projects.GetProject;
using TaskMate.Application.Projects.UpdateProject;
using TaskMate.WebAPI.Common;

namespace TaskMate.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProjectsController(IMediator mediator) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSucceeded
                ? CreatedAtAction(nameof(Get), new { Id = result.Value }, result.Value)
                : MapErrors(result.Errors);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetProjectQuery { Id = Id}, cancellationToken);
            return result.IsSucceeded ? Ok(result.Value) : MapErrors(result.Errors);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProjectCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSucceeded ? NoContent() : MapErrors(result.Errors);
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteProjectCommand { Id = Id }, cancellationToken);
            return result.IsSucceeded ? NoContent() : MapErrors(result.Errors);
        }
    }
}
