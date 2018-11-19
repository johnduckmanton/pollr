/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pollr.Api.Models
{
    public class DbConnectionInfo
    {
        public string State { get; internal set; }
        public string Database { get; internal set; }
        public string  DataSource { get; internal set; }
        public string ServerVersion { get; internal set; }
        public object ConnectionTimeout { get; internal set; }
    }
}
