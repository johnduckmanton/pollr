import { FormControl, FormArray, Validators } from '@angular/forms'
import { PollDefinition } from '../../shared/models/poll-definition.model';

export class PollDefinitionForm {

  name = new FormControl();
  description = new FormControl();
  theme = new FormControl();
  owner = new FormControl();
  isPublished = new FormControl();
  createDate = new FormControl();
  expiryDate = new FormControl();
  tags = new FormControl();
  questions = new FormArray([]);

  constructor(
    pollDefinition: PollDefinition
  ) {
    if (pollDefinition.name) {
      this.name.setValue(pollDefinition.name)
    }
    
    this.name.setValue(pollDefinition.name);
    this.name.setValidators([Validators.required]);
    this.description.setValue(pollDefinition.description);
    this.theme.setValue(pollDefinition.theme);
    this.owner.setValue(pollDefinition.theme);
    this.isPublished.setValue(pollDefinition.isPublished);
    this.createDate.setValue(pollDefinition.createDate);
    this.expiryDate.setValue(pollDefinition.expiryDate);
    this.tags.setValue(pollDefinition.tags);


    if (pollDefinition.questions) {
      this.questions.setValue([pollDefinition.questions])
    }
  }
}
