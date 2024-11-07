import { ChangeDetectorRef, Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { Login } from 'src/app/models/login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = ''; // משתנה להצגת הודעת שגיאה

  @Output() close = new EventEmitter<void>();
  constructor(private authService: AuthService, private cdr: ChangeDetectorRef) { }


  // פונקציה שתשלח את נתוני הלוגין לשרת
  onLogin() {
    if (this.username && this.password) {
      const loginData: Login = {
        username: this.username,
        password: this.password
      };
      this.authService.login(loginData).subscribe(
        (response) => {

          if (response.token) {
            this.authService.saveToken(response.token,response.userName);
            console.log('Login successful');
            this.close.emit();
            this.clearForm();
          }
          // console.log('Login successful');
          // debugger;
          // this.close.emit(); // סגור את המודל
          // this.clearForm(); // מחק את הנתונים (שם משתמש וסיסמה)
          // localStorage.setItem('currentUser', JSON.stringify(response.user));

        },
        (error) => {
          console.error('Login failed', error);
          this.errorMessage = 'ארעה שגיאה. אנא נסה שנית';
        }
      );
    }
  }

  clearForm() {
    this.username = '';
    this.password = '';
  }

  // סגירת המודל על ידי כפתור ה-X
  closeLoginModal() {
    this.close.emit();
  }
}
