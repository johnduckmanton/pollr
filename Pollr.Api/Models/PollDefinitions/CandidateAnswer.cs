/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Pollr.Api.Models.PollDefinitions
{

    public class CandidateAnswer
    {
        public int Id { get; set; }

        public string AnswerText { get; set; }

        public string ImagePath { get; set; }

        public bool IsCorrectAnswer { get; set; } = false;

        public bool IsDisabled { get; set; } = false;

        public int? QuestionDefinitionId { get; set; }
        public QuestionDefinition QuestionDefinition { get; set; }
    }
}