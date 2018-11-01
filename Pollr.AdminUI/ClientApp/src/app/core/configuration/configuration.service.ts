/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Configuration } from './configuration.model';

@Injectable()
export class ConfigurationService {
  private readonly configUrlPath: string = 'ClientConfiguration';
  private configData: Configuration;

  constructor(
    private http: HttpClient) { }

  // Call the ClientConfiguration endpoint, deserialize the response,
  // and store it in this.configData.
  loadConfigurationData(): Promise<Configuration> {


    return this.http.get(`/${this.configUrlPath}`)
      .toPromise()
      .then((response: Configuration) => {
        this.configData = response;
        return this.configData;
      })
      .catch(err => {
        return Promise.reject(err);
      });
  }

  // A helper property to return the config object
  get config(): Configuration {
    return this.configData;
  }
}
