/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Pollr.Api.Models.Polls;
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
        public Answer[] Answers { get; set; }
    }

}
