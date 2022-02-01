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
  wait: boolean = false;
  myOpacity: number = 1;
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
    this.wait = true;
    this.myOpacity = 0.25;
    this.jiraService.getUser(this.loginFrm.value["username"], this.loginFrm.value["password"])
      .subscribe(result => {
        this.wait = false;
        this.user = result;
        this.displayStyle = "none";
        this.closePopup();
      },
        error => {
          this.wait = false;
          console.error(error);
          alert("Usuário ou Senha inválidos");
        }
      );
  }
}

interface User {
  name: string;
  emailAddress: string;
  displayName: string;
}
