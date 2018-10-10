using System;
using System.Threading.Tasks;

namespace Pollr.Api.Hubs
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(string type, string payload);
    }
}
