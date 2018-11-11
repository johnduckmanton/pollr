/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Pollr.Api.Models.Polls
{

    public class Question
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public bool IsDisabled { get; set; } = false;

        public IEnumerable<Answer> Answers { get; set; }

        public int TotalVotes { get; set; }

        public int? PollId { get; set; }
        public Poll Poll { get; set; }
    }

}
