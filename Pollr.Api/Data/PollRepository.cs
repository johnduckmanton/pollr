/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pollr.Api.Exceptions;
using Pollr.Api.Models.PollDefinitions;
using Pollr.Api.Models.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Data
{
    /// <summary>
    /// Handles all data access for Polls
    /// </summary>
    public class PollRepository : IPollRepository
    {
        private readonly PollrContext _context = null;
        private readonly ILogger _logger;

        public PollRepository(DbContextOptions<PollrContext> options,
            ILogger<PollDefinitionRepository> logger)
        {
            _context = new PollrContext(options);
            _logger = logger;
        }

        /// <summary>
        /// Return all polls
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Poll>> GetAllPollsAsync()
        {
            return await _context.Polls
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .ToListAsync();
        }

        /// <summary>
        /// Return all polls with the given status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Poll>> GetPollsByStatusAsync(PollStatus status)
        {
            return await _context.Polls
                .Where(p => p.Status == status)
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .ToListAsync();
        }
               
        /// <summary>
        /// Return the poll matching the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Poll> GetPollAsync(int id)
        {
            var poll = await _context.Polls
                .Where(p => p.Id == id)
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefaultAsync();
                
            if (poll == null)
                throw new PollNotFoundException();

            return poll;
        }

        /// <summary>
        /// Return the poll matching the specified handle
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public async Task<Poll> GetPollByHandleAsync(string handle)
        {
            var poll = await _context.Polls
                .Where(p => p.Handle == handle)
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefaultAsync();

            if (poll == null)
                throw new PollNotFoundException();

            return poll;

        }

        /// <summary>
        /// Add a new poll
        /// </summary>
        /// <param name="poll"></param>
        /// <returns></returns>
        public async Task AddPollAsync(Poll poll)
        {
            await _context.Polls.AddAsync(poll);
            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// Delete a poll
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemovePollAsync(int id)
        {

            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
                throw new PollDefNotFoundException();

            _context.Polls.Remove(poll);
            return (await _context.SaveChangesAsync() > 0);
        }

        /// <summary>
        /// Update a poll
        /// </summary>
        /// <param name="poll"></param>
        /// <returns></returns>
        public async Task<Poll> UpdatePollAsync(Poll poll)
        {
            _context.Polls.Update(poll);

            try
            {
                await _context.SaveChangesAsync();
                return poll;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollExists(poll.Id))
                {
                    throw new PollDefNotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a poll for voting based on the specified poll definition
        /// </summary>
        /// <param name="pollDefinitionId">poll definition id</param>
        /// <param name="handle"></param>
        /// <param name="description"></param>
        /// <param name="name">The name of the poll</param>
        /// <param name="isOpen">If true the poll is created in an open state</param>
        /// <returns></returns>
        public async Task<Poll> CreatePollAsync(string name, int pollDefinitionId, string handle, string description, bool isOpen)
        {
            // First retrieve the poll definition
            var def = await _context.PollDefinitions
                .Where(p => p.Id == pollDefinitionId)
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefaultAsync();

            if (def == null)
                throw new PollDefNotFoundException();

            // Check that the poll has been published
            if (!def.IsPublished)
                throw new PollDefNotPublishedException();

            // Then create a new poll based on the poll definition
            Poll poll = new Poll
            {
                Name = name,
                Description = description,
                Handle = handle,
                PollDefinition= def,
                Status = (isOpen ? PollStatus.Open : PollStatus.Closed),
                PollDate = DateTime.Now,
                CurrentQuestion = 1
            };

            // Create copies of each defined question and its possible answers
            // so that we can store the vote counts for each answer
            List<Question> questionList = new List<Question>();
            foreach (QuestionDefinition question in def.Questions)
            {
                Question q = new Question
                {
                    QuestionText = question.QuestionText,
                    IsDisabled = question.IsDisabled,
                    TotalVotes = 0
                };

                List<Answer> answerList = new List<Answer>();
                foreach (CandidateAnswer answer in question.Answers)
                {
                    Answer a = new Answer
                    {
                        AnswerText = answer.AnswerText,
                        ImagePath = answer.ImagePath,
                        IsDisabled = answer.IsDisabled,
                        IsCorrectAnswer = answer.IsCorrectAnswer,
                        VoteCount = 0
                    };
                    answerList.Add(a);
                }
                q.Answers = answerList.ToArray();
                questionList.Add(q);
            }
            poll.Questions = questionList.ToArray();

            // Create the poll
            await _context.Polls.AddAsync(poll);
            await _context.SaveChangesAsync();

            return poll;

        }

        /// <summary>
        /// Set the poll status to open to voting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> OpenPollAsync(int id)
        {

            var poll = await _context.Polls
                .Where(p => p.Id == id)
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefaultAsync();

            if (poll == null)
                throw new PollNotFoundException();

            poll.Status = PollStatus.Open;
            _context.Entry(poll).State = EntityState.Modified;

            return (await _context.SaveChangesAsync() > 0);

        }

        /// <summary>
        /// Set the poll status to closed to new votes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ClosePollAsync(int id)
        {
            var poll = await _context.Polls
                .Where(p => p.Id == id)
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefaultAsync();

            if (poll == null)
                throw new PollNotFoundException();

            poll.Status = PollStatus.Closed;
            _context.Entry(poll).State = EntityState.Modified;

            return (await _context.SaveChangesAsync() > 0);
        }

        /// <summary>
        /// Increments the poll's current question counter to the next question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Poll> SetNextQuestionAsync(int id)
        {
            // Get the poll
            var poll = await _context.Polls
                .Where(p => p.Id == id)
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefaultAsync();

            if (poll == null)
                throw new PollNotFoundException();

            // Only update the next question if
            // the poll status is 'open'
            if (poll.Status != PollStatus.Open)
                throw new PollClosedException();

            // Check that we don't increment the counter beyond the number of questions in the poll
            int totalQuestions = poll.Questions.Count();
            if (poll.CurrentQuestion >= totalQuestions)
                throw new PollAtLastQuestionException();

            poll.CurrentQuestion++;
            _context.Entry(poll).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return poll;
        }

        /// <summary>
        /// Vote on a question in the poll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns>updated poll</returns>
        public async Task<Poll> VoteAsync(int id, int question, int answer)
        {

            // Get the poll
            var poll = await _context.Polls
                .Where(p => p.Id == id)
                .Include(q => q.Questions)
                .ThenInclude(a => a.Answers)
                .FirstOrDefaultAsync();

            if (poll == null)
                throw new PollNotFoundException();

            // Only allow voting if
            // the poll status is 'open'
            if (poll.Status != PollStatus.Open || poll.CurrentQuestion == 0)
                throw new PollClosedException();

            poll.Questions.ElementAt(question-1).Answers.ElementAt(answer-1).VoteCount++;
            _context.Entry(poll).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return poll;
        }

        private bool PollExists(int id)
        {
            return _context.Polls.Any(e => e.Id == id);
        }

    }
}


