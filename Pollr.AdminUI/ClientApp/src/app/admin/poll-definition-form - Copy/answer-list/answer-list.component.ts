import { Component, Input } from '@angular/core';
import { FormArray } from '@angular/forms';
import { AnswerFormComponent } from '../answer-form/answer-form.component';

@Component({
  selector: 'app-answer-list',
  templateUrl: './answer-list.component.html',
  styleUrls: ['./answer-list.component.css'],
})
export class AnswerListComponent {
  @Input()
  public answerList: FormArray;

  static buildAnswers() {
    return new FormArray([AnswerFormComponent.buildAnswer('')]);
  }

  addAnswer() {
    this.answerList.push(AnswerFormComponent.buildAnswer(''));
  }
}
