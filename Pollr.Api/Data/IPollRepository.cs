/*---------------------------------------------------------------------------------------------
 *  Copyright Async(c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Pollr.Api.Models.Polls;

namespace Pollr.Api.Data
{
    public interface IPollRepository
    {
        Task AddPollAsync(Poll item);
        Task<IEnumerable<Poll>> GetAllPollsAsync();
        Task<IEnumerable<Poll>> GetPollsByStatusAsync(PollStatus status);

        Task<Poll> GetPollAsync(int id);
        Task<Poll> GetPollByHandleAsync(string handl);
        Task<Poll> SetNextQuestionAsync(int id);
        Task<bool> RemovePollAsync(int id);
        Task<Poll> CreatePollAsync(string name, int pollDefinitionId, string handle, bool isOpen);
        Task<bool> OpenPollAsync(int id);
        Task<bool> ClosePollAsync(int id);
        Task<Poll> UpdatePollAsync(Poll poll);
        Task<Poll> VoteAsync(int id, int question, int answer);
    }
}