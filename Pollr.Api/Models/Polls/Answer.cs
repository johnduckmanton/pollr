/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Pollr.Api.Models.Polls
{

    public class Answer
    {
        public int Id { get; set; }

        public string AnswerText { get; set; }

        public string ImagePath { get; set; }

        public bool IsCorrectAnswer { get; set; } = false;

        public bool IsDisabled { get; set; } = false;

        public int VoteCount { get; set; }

        public int? QuestionId { get; set; }
        public Question Question { get; set; }


    }
}
