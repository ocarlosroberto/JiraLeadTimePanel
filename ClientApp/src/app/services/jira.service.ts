import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

var httpOptions = {
  headers: new HttpHeaders({
      'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})

export class JiraService {
  baseUrl: string;
  route: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getUser(username: string, password: string): Observable<User> {
    httpOptions.headers = httpOptions.headers.set('Authorization', 'Basic ' + btoa(username + ':' + password));
    this.route = this.baseUrl + 'jira/user/';

    return this.http.get<User>(this.route + username, httpOptions)
      .pipe(catchError((err) => {
        return throwError(err);
      }));
  }


getCards(): Observable<Card[]> {
  this.route = this.baseUrl + 'jira/CCMCPLATC';

  return this.http.get<Card[]>(this.route, httpOptions)
    .pipe(catchError((err) => {
      return throwError(err);
    }));
}

}

interface User {
  name: string;
  emailAddress: string;
  displayName: string;
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
