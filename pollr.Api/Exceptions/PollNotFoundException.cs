/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Pollr.Api.Exceptions
{
    public class PollNotFoundException: PollrException
    {

        public PollNotFoundException()
        {
            StatusCode = "1001";
            ErrorMessage = ("Poll does not exist");
        }

    }
}
