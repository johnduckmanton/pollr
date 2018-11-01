/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, BehaviorSubject } from 'rxjs';

@Injectable()
export class LoadingService {
    public isLoading = new BehaviorSubject(false);

    constructor() {}
}


