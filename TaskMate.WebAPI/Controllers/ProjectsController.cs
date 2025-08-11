using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Dtos;
using TaskMate.Application.Projects.CreateProject;
using TaskMate.Application.Projects.DeleteProject;
using TaskMate.Application.Projects.GetProject;
using TaskMate.Application.Projects.UpdateProject;
using TaskMate.WebAPI.Responses;

namespace TaskMate.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create(CreateProjectCommand command)
        {
            var result = await _mediator.Send(command);

            var response = ApiResponse<int>.Success(result);

            return CreatedAtAction(nameof(Get), new { Id = result }, response); 
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ApiResponse<ProjectDto>>> Get(int Id)
        {
            var result = await _mediator.Send(new GetProjectQuery { Id = Id});
            return Ok(ApiResponse<ProjectDto>.Success(result));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<object>>> Update(UpdateProjectCommand command)
        {
             await _mediator.Send(command);

            return Ok(ApiResponse<object>.Success(null,"Project Updated Successfully"));
        }


        [HttpDelete("{Id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int Id)
        {
            await _mediator.Send(new DeleteProjectCommand { Id = Id });

            return Ok(ApiResponse<object>.Success(null, "Project Deleted Successfully"));
        }
    }
}
