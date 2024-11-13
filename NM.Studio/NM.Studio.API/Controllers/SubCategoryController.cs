using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NM.Studio.API.Controllers.Base;
using NM.Studio.Domain.CQRS.Commands.SubCategories;
using NM.Studio.Domain.CQRS.Queries.SubCategories;

namespace NM.Studio.API.Controllers
{
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
            var messageResult = await _mediator.Send(subCategoryGetAllQuery);

            return Ok(messageResult);
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var subCategoryGetByIdQuery = new SubCategoryGetByIdQuery
            {
                Id = id
            };
            var messageResult = await _mediator.Send(subCategoryGetByIdQuery);

            return Ok(messageResult);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubCategoryCreateCommand subCategoryCreateCommand)
        {
            var messageView = await _mediator.Send(subCategoryCreateCommand);

            return Ok(messageView);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SubCategoryUpdateCommand subCategoryUpdateCommand)
        {
            var messageView = await _mediator.Send(subCategoryUpdateCommand);

            return Ok(messageView);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] SubCategoryDeleteCommand subCategoryDeleteCommand)
        {
            var messageView = await _mediator.Send(subCategoryDeleteCommand);

            return Ok(messageView);
        }
    }
}
