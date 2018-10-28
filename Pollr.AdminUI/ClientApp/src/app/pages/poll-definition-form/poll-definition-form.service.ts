import { Injectable } from '@angular/core'
import { Observable, BehaviorSubject } from 'rxjs'
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms'
import { PollDefinition } from '../../shared/models/poll-definition.model';
import { QuestionDefinition } from '../../shared/models/question-definition.model';

import { PollDefinitionForm } from './poll-definition-form.model';
import { QuestionDefinitionForm } from './question-form/question-form.model';


@Injectable()
export class PollDefinitionFormService {
  private PollDefinitionForm: BehaviorSubject<
    FormGroup | undefined
    > = new BehaviorSubject(this.fb.group(new PollDefinitionForm(new PollDefinition())))
  PollDefinitionForm$: Observable<FormGroup> = this.PollDefinitionForm.asObservable()

  constructor(private fb: FormBuilder) { }

  addQuestion() {
    const currentPollDefinition = this.PollDefinitionForm.getValue()
    const currentQuestions = currentPollDefinition.get('Questions') as FormArray

    currentQuestions.push(
      this.fb.group(
        new QuestionDefinitionForm(new QuestionDefinition())
      )
    )

    this.PollDefinitionForm.next(currentPollDefinition)
  }

  deleteQuestion(i: number) {
    const currentPollDefinition = this.PollDefinitionForm.getValue()
    const currentQuestions = currentPollDefinition.get('Questions') as FormArray

    currentQuestions.removeAt(i)

    this.PollDefinitionForm.next(currentPollDefinition)
  }
}
