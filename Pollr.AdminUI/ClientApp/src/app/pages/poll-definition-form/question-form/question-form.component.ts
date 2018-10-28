import { Component, OnInit, Input, Output, ChangeDetectionStrategy, EventEmitter } from '@angular/core'
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';

import { PollDefinitionFormService } from '../poll-definition-form.service';
import { QuestionDefinition } from '../../../shared/models/question-definition.model';

@Component({
  selector: 'app-question-form',
  templateUrl: './question-form.component.html',
  styleUrls: ['./question-form.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class QuestionFormComponent implements OnInit {

  @Input() event: QuestionDefinition;
  isEdit: boolean;

  @Input() questionDefinitionForm: FormGroup
  @Input() index: number
  @Output() deleteQuestion: EventEmitter<number> = new EventEmitter()

  constructor() { }

  ngOnInit() {}

  delete() {
    this.deleteQuestion.emit(this.index)
  }

}
