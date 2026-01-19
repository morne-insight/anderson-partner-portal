using AndersonAPI.Application.Common.ConfigOptions;
using AndersonAPI.Application.Common.Interfaces;
using Azure;
using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;

namespace AndersonAPI.Infrastructure.Services
{
    public class AgentService : IAgentService
    {
        private readonly AzureAIOptions _options;

        public AgentService(IOptions<AzureAIOptions> options) => _options = options.Value;

        public async Task<string> RunAsync(string prompt, CancellationToken cancellationToken = default)
        {
            // 1) Build your IChatClient (Azure OpenAI, OpenAI, etc.)
            IChatClient chatClient = BuildChatClient();

            // 2) Construct an agent with instructions/tools
            var agent = new ChatClientAgent(chatClient, new ChatClientAgentOptions
            {
                ChatOptions = new ChatOptions
                {
                    Instructions = "You are a helpful assistant.",
                }
                // You can add tools here if needed
            });

            // 3) return await agent.RunAsync(input);
            var response = await agent.RunAsync(prompt, null, null, cancellationToken);

            return response.Text;
        }

        private IChatClient BuildChatClient()
        {
            if (_options.UseEntraId)
            {
                // Entra ID auth (keyless)
                // For Foundry/Azure endpoints, this is commonly supported; scope differs by service.
                // If you’re hitting Foundry “projects” endpoints you need https://ai.azure.com/.default,
                // but for inference endpoints it may be different. Keep Entra for inference only here.
                TokenCredential credential = new DefaultAzureCredential();

                // Azure.AI.OpenAI supports TokenCredential in newer versions (if yours doesn’t, use ApiKey path below).
                var client = new AzureOpenAIClient(new Uri(_options.Endpoint), credential);
                return client.GetChatClient(_options.DeploymentName).AsIChatClient();
            }
            else
            {
                // API key auth (common for inference)
                var client = new OpenAIClient(
                    new AzureKeyCredential(_options.ApiKey ?? throw new InvalidOperationException("AgentOptions:ApiKey missing")),
                    new OpenAIClientOptions { Endpoint = new Uri(_options.Endpoint) });

                return client.GetChatClient(_options.DeploymentName).AsIChatClient();
            }
        }
    }
}
