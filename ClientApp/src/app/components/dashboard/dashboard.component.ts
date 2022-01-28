import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JiraService } from 'src/app/services/jira.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  public cards: Card[];
  squad: string;

  constructor(private jiraService: JiraService, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.squad = params.get('squad');

      this.jiraService.getCards(this.squad)
        .subscribe(result => {
          this.cards = result;
        });
    });
  }
}

interface Card {
  key: string;
  summary: string;
  status: string;
  issuetype: string;
  assignee: string;
  leadtime: string;
  parent: string;
  bcp: string;
  aggregatetimespent: string;
  bcpXhours: string;
  storyType: string;
  observations: string;
}

