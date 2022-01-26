import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public cards: Card[];
  squad: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private activatedRoute: ActivatedRoute) {
    http.get<Card[]>(baseUrl + 'jira/CCMCPLATC')
      .subscribe(result => {
        this.cards = result;
      },
        error => {
          console.error(error);
        });
  }


  ngOnInit() {

    this.activatedRoute.paramMap.subscribe(params => {
      console.log(params);
      this.squad = params.get('squad');
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
