/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Models
{
    public class QuestionResult
    {
        public string QuestionText { get; set; }
        public int TotalVotes { get; set; }
        public AnswerResult[] Answers { get; set; }
    }

    public class AnswerResult
    {
        public string AnswerText { get; set; }

        public int VoteCount { get; set; }

    }
}
