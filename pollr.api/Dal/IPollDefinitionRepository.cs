/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
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
        Task<IEnumerable<PollDefinition>> GetPollDefinitions(bool publishedOnly);
        Task<PollDefinition> GetPollDefinition(string id);
        Task AddPollDefinition(PollDefinition item);
        Task<bool> RemovePollDefinition(string id);
        Task<bool> UpdatePollDefinition(string id, PollDefinition item);
        Task<bool> RemoveAllPollDefinitions();
        Task<bool> PublishPollDefinition(string id);
    }
}
