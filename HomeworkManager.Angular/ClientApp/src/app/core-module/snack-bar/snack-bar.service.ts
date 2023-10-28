import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from "@angular/material/snack-bar";
import { SuccessSnackBarComponent } from "./components/success-snack-bar/success-snack-bar.component";
import { ErrorSnackBarComponent } from "./components/error-snack-bar/error-snack-bar.component";

@Injectable({
  providedIn: 'root'
})
export class SnackBarService {
  private snackBar = inject(MatSnackBar);

  success(title: string, message: string) {
    this.snackBar.openFromComponent(SuccessSnackBarComponent, {
      verticalPosition: 'top',
      horizontalPosition: 'center',
      duration: 3000,
      data: { title, message }
    });
  }

  error(title: string, message: string) {
    this.snackBar.openFromComponent(ErrorSnackBarComponent, {
      verticalPosition: 'top',
      horizontalPosition: 'center',
      duration: 3000,
      data: { title, message }
    });
  }
}
