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
    public class VoteResult
    {
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; }
    }
}
