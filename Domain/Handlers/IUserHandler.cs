using System.Threading.Tasks;
using JiraLeadTimePanel.Domain.Commands.Request;
using JiraLeadTimePanel.Domain.Commands.Responses;

namespace JiraLeadTimePanel.Domain.Handlers
{
    public interface IUserHandler
    {
        public Task<UserResponse> Handle(string token, UserRequest request);
    }
}