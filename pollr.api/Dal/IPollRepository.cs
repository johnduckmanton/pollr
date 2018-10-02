/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Pollr.Api.Models;

namespace Pollr.Api.Dal
{
    public interface IPollRepository
    {
        Task AddPoll(Poll item);
        Task<IEnumerable<Poll>> GetAllPolls();
        Task<Poll> GetPoll(string id);
        Task<bool> NextQuestion(string id);
        Task<bool> RemoveAllPolls();
        Task<bool> RemovePoll(string id);
        Task<Poll> CreatePoll(string pollDefinitionId, string name, bool isOpen);
        Task<bool> OpenPoll(string id);
        Task<bool> ClosePoll(string id);
        Task<bool> UpdatePoll(string id, Poll item);
        Task<bool> Vote(string id, int questionIdx, int answerIdx);
    }
}