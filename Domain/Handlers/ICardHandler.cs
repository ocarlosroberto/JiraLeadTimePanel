using System.Collections.Generic;
using System.Threading.Tasks;
using JiraLeadTimePanel.Domain.Commands.Request;
using JiraLeadTimePanel.Domain.Commands.Responses;

namespace JiraLeadTimePanel.Domain.Handlers
{
    public interface ICardHandler
    {
        public Task<IEnumerable<CardResponse>> Handle(string token, CardRequest request);
    }
}