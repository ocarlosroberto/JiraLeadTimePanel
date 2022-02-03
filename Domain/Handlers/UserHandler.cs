using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using JiraLeadTimePanel.Domain.Commands.Request;
using JiraLeadTimePanel.Domain.Commands.Responses;

namespace JiraLeadTimePanel.Domain.Handlers
{
    public class UserHandler : IUserHandler
    {
        private HttpClient client;

        public UserHandler()
        {
            client = new HttpClient();
        }

        public async Task<UserResponse> Handle(string token, UserRequest request)
        {
            System.Console.WriteLine(token);
            client.DefaultRequestHeaders.Add("Authorization", token);

            //var url = string.Concat("https://jiracorp.ctsp.prod.cloud.ihf/rest/api/2/user?username=", request.username);
            var url = "http://localhost:3000/user";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Acesso não autorizado");

            return JsonSerializer.Deserialize<UserResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}