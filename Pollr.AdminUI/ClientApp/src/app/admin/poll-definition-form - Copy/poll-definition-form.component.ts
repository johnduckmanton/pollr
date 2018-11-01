import { Component, OnInit, Input } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from "@angular/router";
import { NgForm } from "@angular/forms";

//import { ProductRepository } from "../model/product.repository";

import { PollDefinition } from '../../shared/models/poll-definition.model';
import { PollDataService } from '../../core/poll-data.service';
import { QuestionListComponent } from './question-list/question-list.component';
import { PollDefinitionRepositoryService } from '../poll-definition-repository.service';


@Component({
  selector: 'app-poll-definition-form',
  templateUrl: './poll-definition-form.component.html',
  styleUrls: ['./poll-definition-form.component.css'],
})
export class PollDefinitionFormComponent implements OnInit {

  isEditing: boolean = false;
  pollDefinition: PollDefinition = new PollDefinition();

  pollDefinitionForm: FormGroup;

  constructor(
    private router: Router,
    activeRoute: ActivatedRoute,
    private fb: FormBuilder,
    private repository: PollDefinitionRepositoryService) {

    this.isEditing = activeRoute.snapshot.params["mode"] == "edit";
    if (this.isEditing) {
      Object.assign(this.pollDefinition,
        repository.getPollDefinition(activeRoute.snapshot.params["id"]));
      this.LoadForm();
    }
  }

  ngOnInit() { }

  LoadForm() {

    this.pollDefinitionForm = this.fb.group({
      name: [this.pollDefinition.name, Validators.required],
      description: [this.pollDefinition.description],
      theme: [this.pollDefinition.theme],
      owner: [this.pollDefinition.owner],
      isPublished: [this.pollDefinition.isPublished],
      expiryDate: [this.pollDefinition.expiryDate],
      tags: [this.pollDefinition.tags],
      questions: QuestionListComponent.buildQuestions(),
    });
  }

  onSubmit() {
    console.warn(this.pollDefinitionForm.value);

    // Save the data (Make sure to create a deep copy of the form-model or
    // weird things will happen!
    const newPollDefinition: PollDefinition = Object.assign({}, this.pollDefinitionForm.value);
    //newPollDefinition.questions = Object.assign({}, this.pollDefinitionForm.questions);

    //const newPollDefinition: PollDefinition = {
    //  id: null,
    //  name: this.pollDefinitionForm.value.name,
    //  description: this.pollDefinitionForm.value.description,
    //  theme: this.pollDefinitionForm.value.theme,
    //  owner: this.pollDefinitionForm.value.owner,
    //  createDate: null,
    //  expiryDate: this.pollDefinitionForm.value.expiryDate,
    //  isPublished: this.pollDefinitionForm.value.isPublished,
    //  tags: this.pollDefinitionForm.value.tags,
    //  questions: []
    //}

    this.repository.savePollDefinition(newPollDefinition);

    this.router.navigateByUrl("/admin/poll-definitions");

  }

  get questions(): FormArray {
    return this.pollDefinitionForm.get('questions') as FormArray;
  }
}
