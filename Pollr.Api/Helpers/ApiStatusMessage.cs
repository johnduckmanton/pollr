/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Pollr.Api.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Helpers
{
    internal sealed class ApiStatusMessage
    {

        public ApiStatusMessage()
        {

        }

        public ApiStatusMessage(string statusCode, string errorMessage)
        {
            this.StatusCode = statusCode;
            this.ErrorMessage = errorMessage;
        }

        public string StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public static ApiStatusMessage CreateFromException(PollrException e)
        { 
            return new ApiStatusMessage(e.StatusCode, e.ErrorMessage);

        }
    }
}
