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
import { PollDataService } from '../../core/poll-data.service';
import { AnswerDefinition } from '../../shared/models/answer-definition.model';
import { PollDefinition } from '../../shared/models/poll-definition.model';
import { QuestionDefinition } from '../../shared/models/question-definition.model';

@Component({
  selector: 'app-poll-definition-form',
  templateUrl: './poll-definition-form.component.html',
  styleUrls: ['./poll-definition-form.component.css'],
})
export class PollDefinitionFormComponent implements OnInit {
  id: number;
  isLoading = false;
  isEditing = false;
  pollDefinition: PollDefinition = new PollDefinition();
  pollDefinitionForm: FormGroup;
  sub: Subscription;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private fb: FormBuilder,
    private dataService: PollDataService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.isLoading = true;

    this.id = this.activeRoute.snapshot.params['id'];
    this.isEditing = this.activeRoute.snapshot.params['mode'] === 'edit';

    this.initForm();

    // IF the poll definition id is present and valid
    // then get the poll definition from the server
    // and update the form values
    if (this.isEditing && this.id !== 0) {
      this.dataService
        .getPollDefinition$(this.activeRoute.snapshot.params['id'])
        .subscribe((data: PollDefinition) => {
          this.pollDefinition = data;
          this.updateForm(this.pollDefinition);
          this.isLoading = false;
        });
    } else {
      this.pollDefinition.id = 0;
      this.isLoading = false;
    }
  }

  // Initialise the form with default data
  initForm(): void {
    this.pollDefinitionForm = new FormGroup({
      id: new FormControl(0),
      name: new FormControl('', Validators.required),
      description: new FormControl(''),
      theme: new FormControl(''),
      owner: new FormControl(''),
      createDate: new FormControl(''),
      isPublished: new FormControl(true),
      questions: new FormArray([]),
    });
  }

  // Update the form data fields
  updateForm(pollDefinition: PollDefinition): void {
    this.pollDefinitionForm.patchValue({
      id: pollDefinition.id,
      name: pollDefinition.name,
      description: pollDefinition.description,
      theme: pollDefinition.theme,
      owner: pollDefinition.owner,
      createDate: pollDefinition.createDate,
      isPublished: pollDefinition.isPublished,
    });

    if (this.pollDefinition.questions) {
      this.pollDefinition.questions.forEach((question, questionIndex) => {
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
    return this.pollDefinitionForm.get('questions') as FormArray;
  }

  // Add a question to the form
  addQuestion(question?: QuestionDefinition): void {
    (<FormArray>this.pollDefinitionForm.controls['questions']).push(
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
  addAnswer(questionIndex: number, answer?: AnswerDefinition): void {
    (<FormArray>(
      (<FormGroup>(
        (<FormArray>this.pollDefinitionForm.controls['questions']).controls[questionIndex]
      )).controls['answers']
    )).push(
      new FormGroup({
        id: new FormControl(answer ? answer.id : 0),
        answerText: new FormControl(answer ? answer.answerText : '', Validators.required),
        imagePath: new FormControl(answer ? answer.imagePath : ''),
        isDisabled: new FormControl(answer ? answer.isDisabled : false),
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
    this.pollDefinition = Object.assign({}, this.pollDefinitionForm.value);
    this.pollDefinition.questions = [];

    const questionArray: FormArray = <FormArray>(
      this.pollDefinitionForm.controls['questions']
    );

    for (let i = 0; i < questionArray.length; i++) {
      const q = new QuestionDefinition();
      q.answers = [];
      q.id = questionArray.at(i).get('id').value;
      q.questionText = questionArray.at(i).get('questionText').value;
      q.isDisabled = questionArray.at(i).get('isDisabled').value;
      q.hasCorrectAnswer = questionArray.at(i).get('hasCorrectAnswer').value;

      const answers = questionArray.at(i).get('answers') as FormArray;
      for (let j = 0; j < answers.length; j++) {
        const a = new AnswerDefinition();
        a.id = answers.at(j).get('id').value;
        a.answerText = answers.at(j).get('answerText').value;
        a.imagePath = answers.at(j).get('imagePath').value;
        a.isDisabled = answers.at(j).get('isDisabled').value;

        q.answers.push(a);
      }

      this.pollDefinition.questions.push(q);
    }

    this.dataService.updatePollDefinition$(this.pollDefinition).subscribe(
      () => {
        this.toastr.success(`Poll Definition Updated successfully!`);
        this.router.navigateByUrl('/admin/poll-definitions');
      },
      error => {
        this.toastr.error(error);
      }
    );
  }

  onCancel() {
    this.router.navigateByUrl('/admin/poll-definitions');
  }
}
