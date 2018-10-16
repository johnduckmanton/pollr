import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from '../core/messages/message.service';
import { PollDataService } from '../poll-data.service';
import { Poll } from '../poll.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  isLoading = false;
  polls: Poll[] = [];

  constructor(
    private dataService: PollDataService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.getPolls('all');
  }

  getPolls(status: string): void {
    this.spinner.show();

    if ((status = 'all')) {
      this.dataService.getAllPolls().subscribe(
        data => {
          this.polls = data;
          this.isLoading = false;

          this.spinner.hide();
          if (this.polls.length === 0) {
            this.messageService.add(`There are currently no polls defined.`);
          }
        },
        error => {
          this.spinner.hide();
          this.isLoading = false;
          this.messageService.add('Error retrieving polls data');
        }
      );
    } else {
      this.dataService.getPollsByStatus(status).subscribe(
        data => {
          this.polls = data;
          this.isLoading = false;

          this.spinner.hide();
          if (this.polls.length === 0) {
            this.messageService.add(`There are currently no ${status} polls`);
          }
        },
        error => {
          this.spinner.hide();
          this.isLoading = false;
          this.messageService.add('Error retrieving polls data');
        }
      );
    }
  }

  showInfo(index: number) {
    this.router.navigate(['/poll-details', this.polls[index].id]);
  }

  startPoll(index: number): void {
    this.dataService.openPoll(this.polls[index].id).subscribe(() => {
      this.polls[index].status = 'open';
      this.toastr.success('Your poll has been started. You can now accept votes.');
    });
  }

  stopPoll(index: number): void {
    this.dataService.closePoll(this.polls[index].id).subscribe(() => {
      this.polls[index].status = 'closed';
      this.toastr.warning('Your poll has been stopped. voting is no longer allowed.');
    });
  }

  nextQuestion(index: number): void {
    this.dataService.nextQuestion(this.polls[index].id).subscribe(
      (updatedPoll) => {
        this.polls[index] = updatedPoll;
        console.log(`updated poll: ${updatedPoll}`);
        this.toastr.success(`Question ${updatedPoll.currentQuestion} is now available for votes`);
      }, error => {
        this.toastr.error(error);
      });
  }

  showResults(index: number) {
    this.router.navigate(['/results', this.polls[index].id]);
  }
}
