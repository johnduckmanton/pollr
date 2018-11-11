/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Pollr.Api.Models;
using Pollr.Api.Models.Polls;
using System.Collections.Generic;

namespace Pollr.Api.Helpers
{
    public static class PollHelper
    {
        internal static PollResult GetPollResults(Poll poll)
        {

            PollResult result = new PollResult {
                Id = poll.Id.ToString(),
                Name = poll.Name,
                PollDate = poll.PollDate,
                TotalVotes = 0
            };

            List<QuestionResult> questionList = new List<QuestionResult>();
            foreach (Question question in poll.Questions) {
                QuestionResult q = new QuestionResult {
                    QuestionText = question.QuestionText,
                    TotalVotes = 0
                };

                List<AnswerResult> answerList = new List<AnswerResult>();
                foreach (Answer answer in question.Answers) {
                    AnswerResult a = new AnswerResult {
                        AnswerText = answer.AnswerText,
                        VoteCount = answer.VoteCount
                    };
                    q.TotalVotes += answer.VoteCount;
                    result.TotalVotes += answer.VoteCount;
                    answerList.Add(a);
                }
                q.Answers = answerList.ToArray();
                questionList.Add(q);
            }
            result.Questions = questionList.ToArray();

            return result;

            //var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            //settings.Converters.Add(new StringEnumConverter());
            //return JsonConvert.SerializeObject(result, Formatting.Indented, settings);

        }

    }

}
