/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;

namespace Pollr.Api.Exceptions
{
    [Serializable]
    public class AppConfigErrorException : PollrException
    {

        public AppConfigErrorException()
        {
            StatusCode = "9001";
            ErrorMessage = ("Application Configuraion Error");
        }

        public AppConfigErrorException(string message)
        {
            StatusCode = "9001";
            ErrorMessage = ($"Application Configuration Error: {message}");
        }

    }
}
