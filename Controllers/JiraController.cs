using System.Collections.Generic;
using System.Threading.Tasks;
using JiraLeadTimePanel.Domain.Commands.Request;
using JiraLeadTimePanel.Domain.Commands.Responses;
using JiraLeadTimePanel.Domain.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JiraLeadTimePanel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JiraController : ControllerBase
    {
        private readonly ILogger<JiraController> _logger;

        public JiraController(ILogger<JiraController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("v1/user/{username}")]
        public Task<UserResponse> GetUserResponse([FromServices] IUserHandler handler, string username)
        {
            var token = Request.Headers["Authorization"];

            var request = new UserRequest()
            {
                username = username
            };

            return handler.Handle(token, request);
        }

        [HttpGet]
        [Route("v1/card/{projectName}")]
        public Task<IEnumerable<CardResponse>> GetCardResponse([FromServices] ICardHandler handler, string projectName)
        {
            var token = Request.Headers["Authorization"];

            var request = new CardRequest()
            {
                projectName = projectName
            };

            return handler.Handle(token, request);
        }
    }
}