import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-answer-form',
  templateUrl: './answer-form.component.html',
  styleUrls: ['./answer-form.component.css'],
})
export class AnswerFormComponent implements OnInit {
  @Input()
  public index: number;

  @Input()
  public answer: FormGroup;

  @Output()
  public removed: EventEmitter<number> = new EventEmitter<number>();

  constructor() {}

  static buildAnswer(val: string) {
    return new FormGroup({
      answerText: new FormControl(val, Validators.required),
    });
  }

  ngOnInit() {}
}
