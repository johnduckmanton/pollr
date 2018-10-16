/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Pollr.Api.Exceptions
{
    public class PollAtLastQuestionException : PollrException
    {

        public PollAtLastQuestionException()
        {
            StatusCode = "3";
            ErrorMessage = ("Poll has no more questions");
        }
    }
}
