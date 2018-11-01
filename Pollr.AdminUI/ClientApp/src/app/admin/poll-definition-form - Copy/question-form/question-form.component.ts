import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { AnswerListComponent } from '../answer-list/answer-list.component';

@Component({
  selector: 'app-question-form',
  templateUrl: './question-form.component.html',
  styleUrls: ['./question-form.component.css'],
})
export class QuestionFormComponent implements OnInit {
  @Input()
  public index: number;

  @Input()
  public questionForm: FormGroup;

  @Output()
  public removed: EventEmitter<number> = new EventEmitter<number>();

  constructor() {}

  static buildQuestion(val: string) {
    return new FormGroup({
      questionText: new FormControl(val, Validators.required),
      answers: AnswerListComponent.buildAnswers(),
    });
  }

  ngOnInit() {}

  get answers(): FormArray {
    return this.questionForm.get('answers') as FormArray;
  }
}
