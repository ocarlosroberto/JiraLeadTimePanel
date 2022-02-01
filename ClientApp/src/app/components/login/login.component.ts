import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from "@angular/router"
import { JiraService } from 'src/app/services/jira.service';

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
    { id: "CCMRARRECA", name: "gestão de arrecadação" },
    { id: "CCMRTRANSP", name: "transformação pp e benefício inss" },
  ];

  constructor(private jiraService: JiraService, private formBuilder: FormBuilder, private router: Router) {
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
    this.router.navigate(['/dashboard', this.loginFrm.value["squad"], this.user.displayName.split(" ")[0]]);
  }

  autheticateUser() {
    this.jiraService.getUser(this.loginFrm.value["username"], this.loginFrm.value["password"])
      .subscribe(result => {
        this.user = result;
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
