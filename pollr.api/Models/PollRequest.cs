/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Pollr.Api.Models
{
    public class PollRequest
    {

        public string Name { get; set; }

        public string PollDefinitionId { get; set; }

        public bool IsOpen { get; set; } = false;

    }
}
