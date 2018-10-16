/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Pollr.Api.Exceptions
{
    public class PollrException : Exception
    {

        protected PollrException()
        {

        }

        public PollrException(string statusCode, string errorMessage)
        {
            this.StatusCode = statusCode;
            this.ErrorMessage = errorMessage;
        }

        public string StatusCode { get; set; }

        public string ErrorMessage { get; set; }

    }
}
