using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.CQRS.Queries.SubCategories;

namespace NM.Studio.API.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    [Route("subcategories")]
    public class SubCategoryController : BaseController
    {
        public SubCategoryController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SubCategoryGetAllQuery subCategoryGetAllQuery)
        {
            var businessResult = await _mediator.Send(subCategoryGetAllQuery);

            return Ok(businessResult);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var subCategoryGetByIdQuery = new SubCategoryGetByIdQuery
            {
                Id = id
            };
            var businessResult = await _mediator.Send(subCategoryGetByIdQuery);

            return Ok(businessResult);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubCategoryCreateCommand subCategoryCreateCommand)
        {
            var businessResult = await _mediator.Send(subCategoryCreateCommand);

            return Ok(businessResult);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SubCategoryUpdateCommand subCategoryUpdateCommand)
        {
            var businessResult = await _mediator.Send(subCategoryUpdateCommand);

            return Ok(businessResult);
        }
        
        [HttpPut("restore")]
        public async Task<IActionResult> UpdateIsDeleted([FromBody] SubCategoryRestoreCommand command)
        {
            var businessResult = await _mediator.Send(command);

            return Ok(businessResult);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] SubCategoryDeleteCommand subCategoryDeleteCommand)
        {
            var businessResult = await _mediator.Send(subCategoryDeleteCommand);

            return Ok(businessResult);
        }
    }
}
