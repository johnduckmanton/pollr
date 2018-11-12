/*---------------------------------------------------------------------------------------------
 *  Copyright Async(c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Pollr.Api.Models.PollDefinitions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pollr.Api.Data
{
    public interface IPollDefinitionRepository
    {
        Task<IEnumerable<PollDefinition>> GetPollDefinitionsAsync(bool publishedOnly);
        Task<PollDefinition> GetPollDefinitionAsync(int id);
        Task<PollDefinition> AddPollDefinitionAsync(PollDefinition item);
        Task<bool> RemovePollDefinitionAsync(int id);
        Task<PollDefinition> UpdatePollDefinitionAsync(PollDefinition item);
        Task<bool> SetPublishedStatusAsync(int id, bool isPublished);
    }
}
