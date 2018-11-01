/*---------------------------------------------------------------------------------------------
 *  Copyright Async(c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Pollr.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Dal
{
    public interface IPollDefinitionRepository
    {
        Task<IEnumerable<PollDefinition>> GetPollDefinitionsAsync(bool publishedOnly);
        Task<PollDefinition> GetPollDefinitionAsync(string id);
        Task AddPollDefinitionAsync(PollDefinition item);
        Task<bool> RemovePollDefinitionAsync(string id);
        Task<bool> UpdatePollDefinitionAsync(string id, PollDefinition item);
        Task<bool> PublishPollDefinitionAsync(string id);
        Task<bool> UnpublishPollDefinitionAsync(string id);
    }
}
