import { Injectable } from '@angular/core';
import { PollDefinition } from '../shared/models/poll-definition.model';
import { PollDataService } from '../core/poll-data.service';

@Injectable()
  export class PollDefinitionRepositoryService {
  private pollDefinitions: PollDefinition[] = [];
  private categories: string[] = [];

  constructor(private dataSource: PollDataService) {
    dataSource.getPollDefinitions$().subscribe(data => {
      this.pollDefinitions = data;
    });
  }

  getPollDefinitions(isPublished: boolean = true): PollDefinition[] {
    return this.pollDefinitions
      .filter(p => isPublished == p.isPublished);
  }

  getPollDefinition(id: string): PollDefinition {
    return this.pollDefinitions.find(p => p.id == id);
  }


  savePollDefinition(pollDefinition: PollDefinition) {
    if (pollDefinition.id == null || pollDefinition.id.length == 0) {
      this.dataSource.savePollDefinition$(pollDefinition)
        .subscribe(p => this.pollDefinitions.push(p));
    } else {
      this.dataSource.updatePollDefinition$(pollDefinition)
        .subscribe(p => {
          this.pollDefinitions.splice(this.pollDefinitions.
            findIndex(p => p.id == pollDefinition.id), 1, pollDefinition);
        });
    }
  }

  deletePollDefinition(id: string) {
    this.dataSource.deletePollDefinition$(id).subscribe(p => {
      this.pollDefinitions.splice(this.pollDefinitions.
        findIndex(p => p.id == id), 1);
    })
  }

}
