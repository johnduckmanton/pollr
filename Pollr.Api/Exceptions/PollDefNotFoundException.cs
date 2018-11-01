/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Pollr.Api.Exceptions
{
    public class PollDefNotFoundException: PollrException
    {

        public PollDefNotFoundException()
        {
            StatusCode = "2001";
            ErrorMessage = ("Poll definition does not exist");
        }

    }
}
