import { Component, Input } from '@angular/core';
import { FormArray } from '@angular/forms';
import { QuestionFormComponent } from '../question-form/question-form.component';

@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.component.html',
  styleUrls: ['./question-list.component.css'],
})
export class QuestionListComponent {
  @Input()
  public questionList: FormArray;

  static buildQuestions() {
    return new FormArray([QuestionFormComponent.buildQuestion('')]);
  }

  addQuestion() {
    this.questionList.push(QuestionFormComponent.buildQuestion(''));
  }
}
