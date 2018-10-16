import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  model: any = {};

  pollCode: string;

  constructor(
    private router: Router,
    private route: ActivatedRoute,) { }

  ngOnInit() {
  }

  onSubmit() {
    console.log(this.model);
    const pollCode = this.model.pollCode;
    this.router.navigate(['/vote', pollCode]);

  }
}
