/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Pollr.Api.Exceptions;
using Pollr.Api.Models;
using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<Poll>> GetAllPollsAsync()
        {
            return await _context.Polls
                    .Find(_ => true).ToListAsync();
        }

        /// <summary>
        /// Return all polls with the given status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Poll>> GetPollsByStatusAsync(string status)
        {
            var filter = Builders<Poll>.Filter.Eq("status", status);

            return await _context.Polls
                    .Find(filter).ToListAsync();
        }

        /// <summary>
        /// Return the poll matching the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Poll> GetPollAsync(string id)
        {
            var filter = Builders<Poll>.Filter.Eq(s => s.Id, ObjectId.Parse(id));

            Poll poll = await _context.Polls
                            .Find(filter)
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
            var filter = Builders<Poll>.Filter.Eq(s => s.Handle, handle);

            Poll poll = await _context.Polls
                                .Find(filter)
                                .FirstOrDefaultAsync();

            if (poll == null)
                throw new PollNotFoundException();

            return poll;
        }

        /// <summary>
        /// Add a new poll
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddPollAsync(Poll item)
        {
            item.Id = ObjectId.GenerateNewId();

            await _context.Polls.InsertOneAsync(item);

        }

        /// <summary>
        /// Delete a poll
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemovePollAsync(string id)
        {

            DeleteResult actionResult = await _context.Polls.DeleteOneAsync(
                    Builders<Poll>.Filter.Eq(s => s.Id, ObjectId.Parse(id)));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;

        }

        /// <summary>
        /// Update a poll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePollAsync(string id, Poll item)
        {
            ReplaceOneResult actionResult
                = await _context.Polls
                                .ReplaceOneAsync(n => n.Id.Equals(ObjectId.Parse(id))
                                        , item
                                        , new UpdateOptions { IsUpsert = true });

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;

        }

        /// <summary>
        /// Find and Update a poll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<Poll> FindAndUpdatePollAsync(string id, Poll item)
        {

            var updatedPoll = await _context.Polls.FindOneAndReplaceAsync
                (Builders<Poll>.Filter.Eq(r => r.Id, ObjectId.Parse(id)),
                item,
                new FindOneAndReplaceOptions<Poll>() {
                    IsUpsert = true,
                    ReturnDocument = ReturnDocument.After
                });

            if (updatedPoll == null)
                throw new PollNotFoundException();

            return updatedPoll;

        }


        /// <summary>
        /// Creates a poll for voting based on the specified poll definition
        /// </summary>
        /// <param name="pollDefinitionId">poll definition id</param>
        /// <param name="name">The name of the poll</param>
        /// <param name="isOpen">If true the poll is created in an open state</param>
        /// <returns></returns>
        public async Task<Poll> CreatePollAsync(string name, string pollDefinitionId, string handle, bool isOpen)
        {
            // First retrieve the poll definition
            var builder = Builders<PollDefinition>.Filter;
            var filter = builder.Eq(s => s.Id, ObjectId.Parse(pollDefinitionId));

            PollDefinition def = await _context.PollDefinitions
                .Find(filter)
                .FirstOrDefaultAsync();

            if (def == null) {
                throw new PollNotFoundException();
            }

            // Check that the poll has been published
            if (!def.IsPublished)
                throw new PollDefNotPublishedException();

            // Then create a new poll based on the poll definition
            Poll poll = new Poll {
                Id = ObjectId.GenerateNewId(),
                Name = name,
                Description = def.Description,
                Handle = handle,
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
                        IsDisabled = def.Questions[i].Answers[j].IsDisabled,
                        IsCorrectAnswer = def.Questions[i].Answers[j].IsCorrectAnswer,
                        VoteCount = 0
                    };
                    answerList.Add(a);
                }
                q.Answers = answerList.ToArray();
                questionList.Add(q);
            }
            poll.Questions = questionList.ToArray();

            // Create the poll
            await _context.Polls.InsertOneAsync(poll);
            return poll;

        }

        /// <summary>
        /// Set the poll status to open to voting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> OpenPollAsync(string id)
        {
            // Only set the status to 'open' if it's not set already as to do so
            // would reset the current question index
            var builder = Builders<Poll>.Filter;
            var filter = builder.Eq(s => s.Id, ObjectId.Parse(id))
                & builder.Where(s => s.Status != "open");
            var update = Builders<Poll>.Update
                            .Set(s => s.Status, "open")
                            .Set(s => s.CurrentQuestion, 1);

            UpdateResult actionResult
                    = await _context.Polls.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;

        }

        /// <summary>
        /// Set the poll status to closed to new votes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ClosePollAsync(string id)
        {
            var filter = Builders<Poll>.Filter.Eq(s => s.Id, ObjectId.Parse(id));
            var update = Builders<Poll>.Update
                            .Set(s => s.Status, "closed");

            UpdateResult actionResult
                    = await _context.Polls.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;

        }

        /// <summary>
        /// Increments the poll's current question counter to the next question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Poll> SetNextQuestionAsync(string id)
        {
            // Get the poll
            var filter = Builders<Poll>.Filter.Eq(s => s.Id, ObjectId.Parse(id));
            Poll poll = await _context.Polls.Find(filter).FirstOrDefaultAsync();

            // Only update the next question if
            // the poll status is 'open'
            if (poll == null || poll.Status != "open")
                throw new PollNotFoundException();

            // Check that we don't increment the counter beyond the number of questions in the poll
            int totalQuestions = poll.Questions.Length;
            if (poll.CurrentQuestion >= totalQuestions)
                throw new PollAtLastQuestionException();

            //filter = Builders<Poll>.Filter.Eq(s => s.Id, ObjectId.Parse(id));
            var update = Builders<Poll>.Update.Inc(s => s.CurrentQuestion, 1);

            var updatedPoll = await _context.Polls.FindOneAndUpdateAsync
              (filter,
              update,
              new FindOneAndUpdateOptions<Poll>() {
                  ReturnDocument = ReturnDocument.After
              });

            return updatedPoll;

        }

        /// <summary>
        /// Vote on a question in the poll
        /// </summary>
        /// <param name="id"></param>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns>updated poll</returns>
        public async Task<Poll> VoteAsync(string id, int question, int answer)
        {

            var builder = Builders<Poll>.Filter;
            var filter = Builders<Poll>.Filter.Eq(s => s.Id, ObjectId.Parse(id))
                & builder.Eq(s => s.Status, "open");

            var update = Builders<Poll>.Update.Inc(s => s.Questions[question - 1].Answers[answer - 1].VoteCount, 1);

            var updatedPoll = await _context.Polls.FindOneAndUpdateAsync
              (filter,
              update,
              new FindOneAndUpdateOptions<Poll>() {
                  ReturnDocument = ReturnDocument.After
              });

            if (updatedPoll == null)
                throw new PollNotFoundException();

            return updatedPoll;
        }
    }
}


