using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Pollr.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Helpers
{
    public static class PollHelper
    {
        public static string GetPollResultsAsJson(Poll poll)
        {

            PollResult result = new PollResult {
                Id = poll.Id.ToString(),
                Name = poll.Name,
                PollDate = poll.PollDate,
                TotalVotes = 0
            };

            List<QuestionResult> questionList = new List<QuestionResult>();
            for (int i = 0; i < poll.Questions.Length; i++) {
                QuestionResult q = new QuestionResult {
                    QuestionText = poll.Questions[i].QuestionText,
                    TotalVotes = 0
                };

                List<AnswerResult> answerList = new List<AnswerResult>();
                for (int j = 0; j < poll.Questions[i].Answers.Length; j++) {
                    AnswerResult a = new AnswerResult {
                        AnswerText = poll.Questions[i].Answers[j].AnswerText,
                        VoteCount = poll.Questions[i].Answers[j].VoteCount
                    };
                    q.TotalVotes += poll.Questions[i].Answers[j].VoteCount;
                    result.TotalVotes += poll.Questions[i].Answers[j].VoteCount;
                    answerList.Add(a);
                }
                q.Answers = answerList.ToArray();
                questionList.Add(q);
            }
            result.Questions = questionList.ToArray();

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            settings.Converters.Add(new StringEnumConverter());
            return JsonConvert.SerializeObject(result, Formatting.Indented, settings);

        }
    }
}
