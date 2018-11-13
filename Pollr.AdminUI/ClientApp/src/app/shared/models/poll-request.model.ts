/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
export class PollRequest {
  name: string;
  handle: string;
  description: string;
  status: string;
  pollDefinitionId: number;
}
