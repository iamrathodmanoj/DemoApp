import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/Auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
 model: any = {};
  constructor(public authService: AuthService , private alertifyService: AlertifyService, private router: Router) { }

  ngOnInit() {
  }
   login() {
     this.authService.login(this.model).subscribe(() => {
         this.alertifyService.success('logged in succesfully');
     }, (error) => {
        this.alertifyService.error(error);
     }, () => {
        this.router.navigate(['/members']);
     });
   }

   loggedIn() {
    return this.authService.loggedIn();
   }

   logOut() {
     localStorage.removeItem('token');
     this.alertifyService.message('logout successfully');
     this.router.navigate(['/home']);
   }
}
