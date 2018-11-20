/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';

import { Answer } from '../../shared/models/answer.model';
import { Poll } from '../../shared/models/poll.model';
import { PollDataService } from '../../core/poll-data.service';
import { Question } from '../../shared/models/question.model';

@Component({
  selector: 'app-poll-form',
  templateUrl: './poll-form.component.html',
  styleUrls: ['./poll-form.component.css'],
})
export class PollFormComponent implements OnInit {
  id: number;
  isLoading = false;
  isEditing = false;
  poll: Poll = new Poll();
  pollForm: FormGroup;
  sub: Subscription;

  constructor(
    private activeRoute: ActivatedRoute,
    private dataService: PollDataService,
    private fb: FormBuilder,
    private router: Router,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.isLoading = true;

    this.id = this.activeRoute.snapshot.params['id'];
    this.isEditing = this.activeRoute.snapshot.params['mode'] === 'edit';

    this.initForm();

    // IF the poll id is present and valid
    // then get the poll from the server
    // and update the form values
    if (this.isEditing && this.id !== 0) {
      this.dataService
        .getPoll$(this.activeRoute.snapshot.params['id'])
        .subscribe((data: Poll) => {
          this.poll = data;
          this.updateForm(this.poll);
        });
    } else {
      this.poll.id = 0;
    }

    this.isLoading = false;

  }

  // Initialise the form with default data
  initForm(): void {
    this.pollForm = new FormGroup({
      id: new FormControl(0),
      name: new FormControl('', Validators.required),
      handle: new FormControl('', Validators.required),
      description: new FormControl(''),
      pollDate: new FormControl(''),
      status: new FormControl(),
      questions: new FormArray([]),
    });
  }

  // Update the form data fields with our data
  updateForm(poll: Poll): void {
    this.pollForm.patchValue({
      id: poll.id,
      name: poll.name,
      handle: poll.handle,
      description: poll.description,
      pollDate: poll.pollDate,
      status: poll.status,
    });

    if (this.poll.questions) {
      this.poll.questions.forEach((question, questionIndex) => {
        this.addQuestion(question);

        if (question.answers) {
          question.answers.forEach(answer => {
            this.addAnswer(questionIndex, answer);
          });
        } else {
          this.addAnswer(questionIndex);
        }
      });
    }
  }

  // Getter for the form questions FormArray
  get questions(): FormArray {
    return this.pollForm.get('questions') as FormArray;
  }

  // Add a question to the form
  addQuestion(question?: Question): void {
    (<FormArray>this.pollForm.controls['questions']).push(
      new FormGroup({
        id: new FormControl(question ? question.id : 0),
        questionText: new FormControl(
          question ? question.questionText : '',
          Validators.required
        ),
        isDisabled: new FormControl(question ? question.isDisabled : false),
        hasCorrectAnswer: new FormControl(false),
        answers: new FormArray([]),
      })
    );
  }

  // Add an answer to the form
  addAnswer(questionIndex: number, answer?: Answer): void {
    (<FormArray>(
      (<FormGroup>(
        (<FormArray>this.pollForm.controls['questions']).controls[questionIndex]
      )).controls['answers']
    )).push(
      new FormGroup({
        id: new FormControl(answer ? answer.id : 0),
        answerText: new FormControl(answer ? answer.answerText : '', Validators.required),
        imagePath: new FormControl(answer ? answer.imagePath : ''),
        isDisabled: new FormControl(answer ? answer.isDisabled : false),
        voteCount: new FormControl(answer ? answer.voteCount : 0)
      })
    );
  }

  // Save the updated data to the server
  onSubmit() {

    // Make sure to create a deep copy of
    // the form - model or weird things will happen!
    //
    // TODO: There is probably a more elegent way to do this, but
    // I haven't found it yet
    this.poll = Object.assign({}, this.pollForm.value);
    this.poll.questions = [];

    const questionArray: FormArray = <FormArray>(
      this.pollForm.controls['questions']
    );

    for (let i = 0; i < questionArray.length; i++) {
      const q = new Question();
      q.answers = [];
      q.id = questionArray.at(i).get('id').value;
      q.questionText = questionArray.at(i).get('questionText').value;
      q.isDisabled = questionArray.at(i).get('isDisabled').value;
      // q.hasCorrectAnswer = questionArray.at(i).get('hasCorrectAnswer').value;

      const answers = questionArray.at(i).get('answers') as FormArray;
      for (let j = 0; j < answers.length; j++) {
        const a = new Answer();
        a.id = answers.at(j).get('id').value;
        a.answerText = answers.at(j).get('answerText').value;
        a.imagePath = answers.at(j).get('imagePath').value;
        a.isDisabled = answers.at(j).get('isDisabled').value;

        q.answers.push(a);
      }

      this.poll.questions.push(q);
    }

    this.dataService.updatePoll$(this.poll).subscribe(
      () => {
        this.toastr.success(`Poll Updated successfully!`);
        this.router.navigateByUrl('/admin/polls');
      },
      error => {
        this.toastr.error(error);
      }
    );
  }

  onCancel() {
    this.router.navigateByUrl('/admin/polls');
  }
}
