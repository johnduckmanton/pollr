/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit, Input } from '@angular/core';
import { Question } from '../../shared/models/question.model';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-result-chart',
  templateUrl: './result-chart.component.html',
  styleUrls: ['./result-chart.component.css']
})
export class ResultChartComponent implements OnInit {

  //@Input() question: Question;
  @Input() question: Observable<Question>;
  @Input() displayQuestionText: boolean = false;

  constructor() { }

  ngOnInit() {

  }

}
