/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Pollr.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Dal
{
    public class PollRepository : IPollRepository
    {
        private readonly DatabaseContext _context = null;

        public PollRepository(IOptions<Settings> settings)
        {
            _context = new DatabaseContext(settings);
        }

        /// <summary>
        /// Return all polls
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Poll>> GetAllPolls()
        {
            try {
                return await _context.Polls
                        .Find(_ => true).ToListAsync();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Return all polls with the given status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Poll>> GetPollsByStatus(string status)
        {
            var filter = Builders<Poll>.Filter.Eq("status", status);

            try {
                return await _context.Polls
                        .Find(filter).ToListAsync();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Return the poll matching the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Poll> GetPoll(string id)
        {
            var filter = Builders<Poll>.Filter.Eq(s => s.Id, ObjectId.Parse(id));

            try {
                return await _context.Polls
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Return the poll matching the specified handle
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public async Task<Poll> GetPollByHandle(string handle)
        {
            var filter = Builders<Poll>.Filter.Eq(s => s.Handle, handle);

            try {
                return await _context.Polls
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Add a new poll
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddPoll(Poll item)
        {
            try {
                item.Id = ObjectId.GenerateNewId();

                await _context.Polls.InsertOneAsync(item);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Delete a poll
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemovePoll(string id)
        {
            try {
                DeleteResult actionResult = await _context.Polls.DeleteOneAsync(
                        Builders<Poll>.Filter.Eq(s => s.Id, ObjectId.Parse(id)));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Update a poll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePoll(string id, Poll item)
        {
            try {
                ReplaceOneResult actionResult
                    = await _context.Polls
                                    .ReplaceOneAsync(n => n.Id.Equals(ObjectId.Parse(id))
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Creates a poll for voting based on the specified poll definition
        /// </summary>
        /// <param name="pollDefinitionId">poll definition id</param>
        /// <param name="name">The name of the poll</param>
        /// <param name="isOpen">If true the poll is created in an open state</param>
        /// <returns></returns>
        public async Task<Poll> CreatePoll(string name, string pollDefinitionId, bool isOpen)
        {
            // First retrieve the poll definition
            var builder = Builders<PollDefinition>.Filter;
            var filter = builder.Eq(s => s.Id, ObjectId.Parse(pollDefinitionId));
            //& builder.Eq(s => s.IsPublished, true);

            PollDefinition def = await _context.PollDefinitions
                .Find(filter)
                .FirstOrDefaultAsync();

            if (def == null) {
                throw new Exception($"Poll definition {pollDefinitionId} not found, or not published");
            }

            // Then create a new poll based on the poll definition
            Poll poll = new Poll {
                Id = ObjectId.GenerateNewId(),
                Name = name,
                Description = def.Description,
                PollDefinitionId = def.Id,
                Status = (isOpen ? "open" : "closed"),
                PollDate = DateTime.Now,
                CurrentQuestion = 1
            };

            // Create copies of each defined question and its possible answers
            // so that we can store the vote counts for each answer
            List<Question> questionList = new List<Question>();
            for (int i = 0; i < def.Questions.Length; i++) {
                Question q = new Question {
                    QuestionText = def.Questions[i].QuestionText,
                    IsDisabled = def.Questions[i].IsDisabled

                };

                List<Answer> answerList = new List<Answer>();
                for (int j = 0; j < def.Questions[i].Answers.Length; j++) {
                    Answer a = new Answer {
                        AnswerText = def.Questions[i].Answers[j].AnswerText,
                        ImagePath = def.Questions[i].Answers[j].ImagePath,
                        VoteCount = 0
                    };
                    answerList.Add(a);
                }
                q.Answers = answerList.ToArray();
                questionList.Add(q);
            }
            poll.Questions = questionList.ToArray();

            // Create the poll
            try {
                await _context.Polls.InsertOneAsync(poll);
                return poll;

            }
            catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }

        }

        /// <summary>
        /// Set the poll status to open to voting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> OpenPoll(string id)
        {
            // Only set the status to 'open' if it's not set already as to do so
            // would reset the current question index
            var builder = Builders<Poll>.Filter;
            var filter = builder.Eq(s => s.Id, ObjectId.Parse(id))
                & builder.Where(s => s.Status != "open");
            var update = Builders<Poll>.Update
                            .Set(s => s.Status, "open")
                            .Set(s => s.CurrentQuestion, 1);

            try {
                UpdateResult actionResult
                     = await _context.Polls.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }

        }

        /// <summary>
        /// Set the poll status to closed to new votes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ClosePoll(string id)
        {
            var filter = Builders<Poll>.Filter.Eq(s => s.Id, ObjectId.Parse(id));
            var update = Builders<Poll>.Update
                            .Set(s => s.Status, "closed");

            try {
                UpdateResult actionResult
                     = await _context.Polls.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex) {
                // log or manage the exception
                throw ex;
            }

        }

        /// <summary>
        /// Increments the poll's current question counter to the next question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> SetNextQuestion(string id)
        {
            // Only update the next question if 
            // the poll status is 'open' 
            var builder = Builders<Poll>.Filter;
            var filter = builder.Eq(s => s.Id, ObjectId.Parse(id))
                & builder.Eq(s => s.Status, "open");

            // TODO: Check that we don't increment the counter beyond the number of questions in the poll

            var update = Builders<Poll>.Update
                            .Inc(s => s.CurrentQuestion, 1);

            try {
                UpdateResult actionResult
                     = await _context.Polls.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex) {
                throw ex;
            }

        }

        /// <summary>
        /// Vote on a question in the poll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public async Task<bool> Vote(string id, int question, int answer)
        {
            // We only allow votes on polls that are 'open'
            var builder = Builders<Poll>.Filter;
            var filter = builder.Eq(s => s.Id, ObjectId.Parse(id))
                & builder.Eq(s => s.Status, "open");
            var update = Builders<Poll>.Update
                            .Inc(s => s.Questions[question - 1].Answers[answer - 1].VoteCount, 1);

            try {
                UpdateResult actionResult
                     = await _context.Polls.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex) {
                throw ex;
            }

        }

    }
}


