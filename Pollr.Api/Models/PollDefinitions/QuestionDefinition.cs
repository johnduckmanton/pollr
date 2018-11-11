/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Pollr.Api.Models.PollDefinitions

{

    public class QuestionDefinition
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public bool HasCorrectAnswer { get; set; } = false;

        public bool IsDisabled { get; set; } = false;

        public IEnumerable<CandidateAnswer> Answers { get; set; }

        public int? PollDefinitionId { get; set; }
        public PollDefinition PollDefinition { get; set; }
    }

}