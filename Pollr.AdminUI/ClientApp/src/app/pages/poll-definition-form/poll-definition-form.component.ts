import { Component, OnInit, OnDestroy } from '@angular/core'
import { FormGroup, FormArray } from '@angular/forms'
import { PollDefinitionFormService } from './poll-definition-form.service'
import { Subscription } from 'rxjs'

@Component({
  selector: 'app-poll-definition-form',
  templateUrl: './poll-definition-form.component.html',
  styleUrls: ['./poll-definition-form.component.css']
})
export class PollDefinitionFormComponent implements OnInit, OnDestroy {
  PollDefinitionForm: FormGroup
  PollDefinitionFormSub: Subscription
  formInvalid: boolean = false;
  Questions: FormArray

  constructor(private PollDefinitionFormService: PollDefinitionFormService) { }

  ngOnInit() {
    this.PollDefinitionFormSub = this.PollDefinitionFormService.PollDefinitionForm$
      .subscribe(PollDefinition => {
        this.PollDefinitionForm = PollDefinition
        this.Questions = this.PollDefinitionForm.get('questions') as FormArray
      })
  }

  ngOnDestroy() {
    this.PollDefinitionFormSub.unsubscribe()
  }

  addQuestion() {
    this.PollDefinitionFormService.addQuestion()
  }

  deleteQuestion(index: number) {
    this.PollDefinitionFormService.deleteQuestion(index)
  }

  savePollDefinition() {
    console.log('PollDefinition saved!')
    console.log(this.PollDefinitionForm.value)
  }
}

