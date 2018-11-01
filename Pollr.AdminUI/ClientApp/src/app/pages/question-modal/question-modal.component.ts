/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap'

import { QuestionDefinition } from '../../shared/models/question-definition.model';



@Component({
  selector: 'app-question-modal',
  templateUrl: './question-modal.component.html',
  styleUrls: ['./question-modal.component.css']
})
export class QuestionModalComponent implements OnInit {

  //@Input() question: QuestionDefinition;
  @Input() myForm: FormGroup;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder
  ) {
    this.createForm();
  }

  ngOnInit() {
  }

  private createForm() {
    //this.myForm = this.formBuilder.group({
    //  username: '',
    //  password: ''
    //});
  }
  private submitForm() {
    this.activeModal.close(this.myForm.value);
  }

  closeModal() {
    this.activeModal.close('Modal Closed');
  }

}
