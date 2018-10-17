/*---------------------------------------------------------------------------------------------
 *  Copyright Async(c) John Duckmanton.
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
        Task AddPollAsync(Poll item);
        Task<IEnumerable<Poll>> GetAllPollsAsync();
        Task<IEnumerable<Poll>> GetPollsByStatusAsync(string status);

        Task<Poll> GetPollAsync(string id);
        Task<Poll> GetPollByHandleAsync(string handl);
        Task<Poll> SetNextQuestionAsync(string id);
        Task<bool> RemovePollAsync(string id);
        Task<Poll> CreatePollAsync(string pollDefinitionId, string name, string handle, bool isOpen);
        Task<bool> OpenPollAsync(string id);
        Task<bool> ClosePollAsync(string id);
        Task<bool> UpdatePollAsync(string id, Poll item);
        Task<Poll> VoteAsync(string id, int question, int answer);
    }
}