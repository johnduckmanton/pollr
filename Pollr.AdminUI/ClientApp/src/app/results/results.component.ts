import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { NgxSpinnerService } from 'ngx-spinner';

import { ActivatedRoute } from '@angular/router';
import { PollDataService } from '../poll-data.service';
import { MessageService } from '../message.service';


@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css']
})
export class ResultsComponent implements OnInit {

  public barChartData: any[] = [0, 0, 0];
  public renderChart: boolean = false;

  public value1 = 0;
  public value2 = 0;
  public value3 = 0;
  public maxValue = 0;
 
  constructor(
    private spinner: NgxSpinnerService,
    private route: ActivatedRoute,
    private location: Location,
    private dataService: PollDataService,
    private messageService: MessageService
  ) { }

  public results;



  ngOnInit() {
    this.spinner.show();
    const id: string = this.route.snapshot.paramMap.get('id');

    this.results = this.dataService.getPollResults(id).subscribe(
      results => {
        this.results = results;
        console.log(results);

        // Need to do object.assign hack + DOM chart object update
        // in order to force the chart labels to update

        this.value1 = results.questions[0].answers[0].voteCount;
        this.value2 = results.questions[0].answers[1].voteCount;
        this.value3 = results.questions[0].answers[2].voteCount;
        this.maxValue = results.questions[0].totalVotes;

      });
    this.spinner.hide();
  }


  // events
  public chartClicked(e: any): void {
  }

  public chartHovered(e: any): void {
  }

}
