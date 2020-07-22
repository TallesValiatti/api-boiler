using System;
using System.Threading.Tasks;
using Api.Core.Dto.Requests;
using Api.Core.Tasks.Commands.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace Api.Web.Controllers.Security
{
    [Route("api/[controller]")]
    //[EnableCors("AllowAllOrigins")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] CreateLoginRequest request)
        {
            var result = await _mediator.Send(new CreateLoginCommand(request));
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Refresh")]
        public async Task<IActionResult> RefreshLogin([FromBody] CreateRefreshLoginRequest request)
        {
            var result = await _mediator.Send(new CreateRefreshLoginCommand(request));
            return Ok(result);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TesteLoginAuth()
        {
            throw new Exception("Erro ao processar dados TesteLoginAuth");
            return Ok("teste");
        }
    }
}
