/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Pollr.Api.Exceptions
{
    public class PollDefNotPublishedException : PollrException
    {

        public PollDefNotPublishedException()
        {
            StatusCode = "2002";
            ErrorMessage = ("Poll has not been published");
        }

    }
}
