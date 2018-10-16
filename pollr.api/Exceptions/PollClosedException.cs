/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Pollr.Api.Exceptions
{
    public class PollClosedException : PollrException
    {

        public PollClosedException()
        {
            StatusCode = "2";
            ErrorMessage = ("Poll does not exist or is not open");
        }

    }
}

