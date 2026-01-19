using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndersonAPI.Application.Common.Interfaces
{
    public interface IAgentService
    {
        Task<string> RunAsync(string prompt, CancellationToken cancellationToken = default);
    }
}
