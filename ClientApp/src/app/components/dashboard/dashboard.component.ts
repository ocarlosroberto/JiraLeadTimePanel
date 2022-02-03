import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JiraService } from 'src/app/services/jira.service';
import { transition, animate, trigger, state, style } from '@angular/animations';

import { MatTableDataSource } from '@angular/material';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})

export class DashboardComponent implements OnInit {
  cards: Card[];
  squad: string;
  authenticated: boolean = false;
  fullName: string;
  observations: string;

  wait: boolean = false;
  myOpacity: number = 1;

  epicsSource = new MatTableDataSource<Card>();
  displayedEpicsColumns: string[] = ['key', 'name', 'status', 'leadtime'];

  issuesSource = new MatTableDataSource<Card>();
  displayedIssuesColumns: string[] = ['parent', 'key', 'issuetype', 'summary', 'leadtime', 'bcpXhours', 'assignee', 'status'];

  constructor(private jiraService: JiraService, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.paramMap.subscribe(params => {
      this.squad = params.get('squad');
      this.fullName = params.get('name');

      if (this.squad != null && this.fullName != null)
        this.authenticated = true;
      else
        this.authenticated = false;

      this.wait = true;
      this.myOpacity = 0.25;

      this.jiraService.getCards(this.squad)
        .subscribe(result => {
          console.log(result);
          this.wait = false;
          this.myOpacity = 1;

          this.cards = result;

          this.epicsSource = new MatTableDataSource(this.cards.filter(card => card.issuetype === 'Epic'));
          this.issuesSource = new MatTableDataSource(this.cards.filter(card => card.issuetype !== 'Epic'));
        },
          error => {
            this.wait = false;
            this.myOpacity = 1;
            console.error(error);
            alert("Erro ao buscar cards");
          }
        );
    });
  }

  getObservations(key: string): string {
    return localStorage.getItem(key);
  }

  saveLocalStorage(key: string) {
    localStorage.setItem(key, this.observations);
    this.observations = "";
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

