/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-create-poll-definition',
  templateUrl: './create-poll-definition.component.html',
  styleUrls: ['./create-poll-definition.component.css']
})
export class CreatePollDefinitionComponent implements OnInit {
  pageTitle = 'Create Poll Definition';

  constructor(private title: Title) { }

  ngOnInit() {
    this.title.setTitle(this.pageTitle);
  }


}
