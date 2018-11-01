/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  constructor(private toastr: ToastrService) {}

  messages: string[] = [];

  add(message: string) {
    this.messages.push(message);
    this.toastr.info(message);
  }

  clear() {
    this.messages = [];
  }
}
