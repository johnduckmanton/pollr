/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;

namespace Pollr.Api.Exceptions
{
    [Serializable]
    public class PollAtLastQuestionException : PollrException
    {

        public PollAtLastQuestionException()
        {
            StatusCode = "1003";
            ErrorMessage = ("Poll has no more questions");
        }
    }
}
