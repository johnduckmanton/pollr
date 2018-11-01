import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, BehaviorSubject } from 'rxjs';

@Injectable()
export class LoadingService {
    public isLoading = new BehaviorSubject(false);

    constructor() {}
}


