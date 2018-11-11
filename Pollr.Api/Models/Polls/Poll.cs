/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using Pollr.Api.Models.PollDefinitions;

namespace Pollr.Api.Models.Polls
{
    public class Poll
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Handle { get; set; }

        public string Description { get; set; }

        public PollStatus Status { get; set; } = PollStatus.Open;

        public bool IsPublished { get; set; } = false;

        public DateTime? ExpiryDate { get; set; }

        public DateTime PollDate { get; set; } = DateTime.Now;

        public short CurrentQuestion { get; set; } = 1;

        public IEnumerable<Question> Questions { get; set; }

        public PollDefinition PollDefinition { get; set; }

    }

    public enum PollStatus
    {
        Undefined = 0,
        Open,
        Closed
    }
}
