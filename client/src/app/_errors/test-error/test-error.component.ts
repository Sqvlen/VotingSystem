import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent {
  private _baseUrl = 'https://localhost:5001/api/'
  _validationErrors: string[] = [];

  constructor(private http: HttpClient) { }

  // status code - 404
  getNotFoundError(): void {
    this.http.get(this._baseUrl + 'buggy/not-found').subscribe
      ({
        next: response => console.log(response),
        error: error => console.log(error),
      });
  }

  // status code - 400
  getBadRequestError(): void {
    this.http.get(this._baseUrl + 'buggy/badrequest').subscribe
      ({
        next: response => console.log(response),
        error: error => console.log(error),
      });
  }

  // status code - 500
  getServerError(): void {
    this.http.get(this._baseUrl + 'buggy/server-error').subscribe
      ({
        next: response => console.log(response),
        error: error => console.log(error),
      });
  }

  // status code - 401
  getAuthError(): void {
    this.http.get(this._baseUrl + 'buggy/testauth').subscribe
      ({
        next: response => console.log(response),
        error: error => console.log(error)
      });
  }

  // status code - 400 (validation)
  getValidaitonError(): void {
    this.http.post(this._baseUrl + 'account/register', {}).subscribe
      ({
        next: response => console.log(response),
        error: error => {
          console.log(error);
          this._validationErrors = error;
        },
      });
  }
}
