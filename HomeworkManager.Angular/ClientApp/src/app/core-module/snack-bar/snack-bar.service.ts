import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from "@angular/material/snack-bar";
import { SuccessSnackBarComponent } from "./components/success-snack-bar/success-snack-bar.component";
import { ErrorSnackBarComponent } from "./components/error-snack-bar/error-snack-bar.component";
import { InfoSnackBarComponent } from "./components/info-snack-bar/info-snack-bar.component";

@Injectable({
  providedIn: 'root'
})
export class SnackBarService {
  private snackBar = inject(MatSnackBar);

  info(title: string, message: string = '') {
    this.snackBar.openFromComponent(InfoSnackBarComponent, {
      data: { title, message }
    })
  }

  success(title: string, message: string = '') {
    this.snackBar.openFromComponent(SuccessSnackBarComponent, {
      data: { title, message }
    });
  }

  error(title: string, message: string = '') {
    this.snackBar.openFromComponent(ErrorSnackBarComponent, {
      data: { title, message }
    });
  }
}
