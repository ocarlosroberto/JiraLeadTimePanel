import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-progress-bar',
  templateUrl: './progress-bar.component.html',
  styleUrls: ['./progress-bar.component.css']
})
export class ProgressBarComponent implements OnInit {

  @Input() progress!: number;
  @Input() total!: string;
  color!: string;
  value!: string;

  constructor() { }

  ngOnInit() {
    console.log('Progress ' + this.progress.toString() + '\nTotal ' + this.total);
    this.value = this.progress.toString();

    if (!this.progress)
      this.progress = 0;

    if (this.total === '0')
      this.total = this.progress.toString();
    else if (!this.total)
      this.total = '100';

    if (this.progress.toString() > this.total)
    {
      this.progress =  100;
      this.total = '100';
    }

    this.progress = this.progress / Number(this.total) * 100;

    if (isNaN(this.progress))
      this.color = 'black';
    else if (this.progress < 15)
      this.color = 'initial';
    else if (this.progress < 55)
      this.color = 'green'
    else if (this.progress < 95)
      this.color = 'yellow'
    else
      this.color = 'red';

  }
}
