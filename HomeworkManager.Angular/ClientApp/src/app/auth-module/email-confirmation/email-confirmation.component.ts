import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { AuthService } from "../../services";
import { NavigationItems, SnackBarService } from "../../core-module";

@Component({
  selector: 'hwm-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.scss']
})
export class EmailConfirmationComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private snackBarService = inject(SnackBarService);
  protected readonly NavigationItems = NavigationItems;
  authenticated = true;
  confirmed = false;

  ngOnInit() {
    this.route.queryParamMap
      .subscribe(params => {
        this.authService.authenticate()
          .subscribe({
            next: () => {
              const token = params.get('token');
              if (token) {
                this.authService.confirmEmail(token)
                  .subscribe({
                    next: success => {
                      this.confirmed = success;
                    },
                    error: error => {
                      this.snackBarService.error('Cannot confirm email', error.error);
                    }
                  });
              }
            },
            error: error => {
              this.authenticated = false;
            }
          });
      });
  }
}
