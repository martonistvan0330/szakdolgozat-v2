import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { AuthService } from "../../../core-module";
import {Router} from "@angular/router";

@Component({
  selector: 'hwm-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent {
  @Input() isMobile: boolean | null = false;
  @Output() toggleNavbar = new EventEmitter<void>();
  authService = inject(AuthService);
  private router = inject(Router);

  onClick() {
    this.toggleNavbar.emit();
  }

  logout() {
    this.authService.logout()
      .subscribe(
        () => {
          this.router.navigateByUrl('/login');
        }
      )
  }
}
