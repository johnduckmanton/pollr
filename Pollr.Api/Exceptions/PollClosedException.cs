﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;

namespace Pollr.Api.Exceptions
{
    [Serializable]
    public class PollClosedException : PollrException
    {

        public PollClosedException()
        {
            StatusCode = "1002";
            ErrorMessage = ("Poll does not exist or is not open");
        }

    }
}

