/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit, Input } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

import { PollDefinition } from '../../shared/models/poll-definition.model';
import { QuestionDefinition } from '../../shared/models/question-definition.model';
import { AnswerDefinition } from '../../shared/models/answer-definition.model';
import { PollDataService } from '../../core/poll-data.service';
import { PollDefinitionRepositoryService } from '../poll-definition-repository.service';


@Component({
  selector: 'app-poll-definition-form',
  templateUrl: './poll-definition-form.component.html',
  styleUrls: ['./poll-definition-form.component.css'],
})
export class PollDefinitionFormComponent implements OnInit {

  isEditing = false;
  pollDefinition: PollDefinition = new PollDefinition();

  pollDefinitionForm: FormGroup;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private fb: FormBuilder,
    private repository: PollDefinitionRepositoryService) {

      this.isEditing = activeRoute.snapshot.params['mode'] === 'edit';

  }

  ngOnInit() {

    // Fetch the poll definition id if it is present then get the
    // poll definition and pass it to the initForm method.
    if (this.isEditing) {
      Object.assign(this.pollDefinition,
        this.repository.getPollDefinition(this.activeRoute.snapshot.params['id']));
      this.initForm(this.pollDefinition);
    } else {
      this.initForm();
    }
  }

  // Initialise the form with default data if adding, or
  // copy the pollDefinition data if editing
  initForm(pollDefinition?: PollDefinition): void {

    this.pollDefinitionForm = new FormGroup({
      name: new FormControl(pollDefinition ? pollDefinition.name : '', Validators.required),
      description: new FormControl(pollDefinition ? pollDefinition.description : ''),
      theme: new FormControl(pollDefinition ? pollDefinition.theme : ''),
      owner: new FormControl(pollDefinition ? pollDefinition.owner : ''),
      createDate: new FormControl(pollDefinition ? pollDefinition.createDate : ''),
      expiryDate: new FormControl(pollDefinition ? pollDefinition.expiryDate : ''),
      isPublished: new FormControl(pollDefinition ? pollDefinition.isPublished : ''),
      tags: new FormControl(pollDefinition ? pollDefinition.tags : ''),
      questions: new FormArray([])
    });

    if (!pollDefinition) {
      this.addQuestion();
      this.addAnswer(0);
    } else {
      if (pollDefinition.questions) {
        pollDefinition.questions.forEach((question, questionIndex) => {
          this.addQuestion(question);

          if (question.answers) {
            question.answers.forEach((answer) => {
              this.addAnswer(questionIndex, answer);
            });
          } else {
            this.addAnswer(questionIndex);
          }
        });
      }
      else {
        this.addQuestion();
        this.addAnswer(0);
      }
    }
  }

  get questions(): FormArray {
    return this.pollDefinitionForm.get('questions') as FormArray;
  }

  addQuestion(question?: QuestionDefinition): void {

    const answers = new FormArray([]);
    const questionText = question ? question.questionText : '';

    (<FormArray>this.pollDefinitionForm.controls['questions']).push(
      new FormGroup({
        questionText: new FormControl(questionText, Validators.required),
        answers: answers
      })
    );
  }

  addAnswer(questionIndex: number, answer?: AnswerDefinition): void {

    const answerText = answer ? answer.answerText : '';

    (<FormArray>(<FormGroup>(<FormArray>this.pollDefinitionForm.controls['questions'])
      .controls[questionIndex]).controls['answers']).push(
        new FormGroup({
          answerText: new FormControl(answerText, Validators.required),
        })
      );
  }

  onSubmit() {
    console.warn(this.pollDefinitionForm.value);

    const newPollDefinition: PollDefinition = Object.assign({}, this.pollDefinitionForm.value);
    newPollDefinition.questions = [];

    // Save the data (Make sure to create a deep copy of
    // the form - model or weird things will happen!
    // There is probably a more elegent way to do this, but
    // I haven't found it yet
    const questionArray: FormArray = (<FormArray>this.pollDefinitionForm.controls['questions']);

    for (let i = 0; i < questionArray.length; i++) {
      const q = new QuestionDefinition();
      q.answers = [];
      q.questionText = questionArray.at(i).get('questionText').value;

      const answers = questionArray.at(i).get('answers') as FormArray;
      for (let j = 0; j < answers.length; j++) {
        const a = new AnswerDefinition();
        a.answerText = answers.at(j).get('answerText').value;

        q.answers.push(a);
      }

      newPollDefinition.questions.push(q);
    }

    console.log(newPollDefinition);

    this.repository.savePollDefinition(newPollDefinition);

    this.router.navigateByUrl("/admin/poll-definitions");

  }

}
