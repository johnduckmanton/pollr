/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Pollr.Api.Models.PollDefinitions
{
    public class PollDefinition
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Theme { get; set; }

        public string Owner { get; set; }

        public bool IsPublished { get; set; } = false;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public IEnumerable<QuestionDefinition> Questions { get; set; }

    }

}