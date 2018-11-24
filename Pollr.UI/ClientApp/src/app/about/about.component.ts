/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit } from '@angular/core';

import { ConfigurationService } from '../core/configuration/configuration.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {

  appVersion = this.configService.config.appVersion;

  constructor(public configService: ConfigurationService) {
  }

  ngOnInit() {
  }

}
