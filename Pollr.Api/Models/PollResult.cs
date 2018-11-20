/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Pollr.Api.Models
{
    public class PollResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime PollDate { get; set; }
        public int TotalVotes { get; set; }
        public QuestionResult[] Questions { get; set; }
        public short CurrentQuestion { get; set; }

    }


}
