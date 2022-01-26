import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from "@angular/router"
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginFrm!: FormGroup;
  user: User;
  squads = [
    { id: "CCMCPLATC", name: "cash plataformas comerciais" },
    { id: "SQUAD2", name: "squad 2" },
    { id: "SQUAD3", name: "squad 3" },
  ];
  route: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private formBuilder: FormBuilder, private router: Router) {
    this.route = baseUrl + 'jira/user/';
  }

  ngOnInit() {
    this.loginFrm = this.formBuilder.group({
      username: new FormControl(""),
      password: new FormControl(""),
      squad: new FormControl("")
    })

    this.openPopup();
  }

  displayStyle = "none";

  openPopup() {
    this.displayStyle = "block";
  }
  closePopup() {
    this.displayStyle = "none";
    this.router.navigate(['/fetch-data', this.loginFrm.value["squad"]]);
  }

  autheticateUser() {
    this.http.get<User>(this.route + this.loginFrm.value["username"])
      .subscribe(result => {
        this.user = result;
        console.log(this.user);

        this.displayStyle = "none";
        this.closePopup();
      },
        error => {
          console.error(error);
          alert("Invalid User Name or Password");
        }
      );
  }
}

interface User {
  name: string;
  emailAddress: string;
  displayName: string;
}
