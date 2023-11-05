import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { NavigationItems } from "../../../core-module";
import { Router } from "@angular/router";
import { AuthService } from "../../../services";

@Component({
  selector: 'hwm-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent {
  private router = inject(Router);
  protected readonly NavigationItems = NavigationItems;
  @Input() isMobile: boolean | null = false;
  @Output() toggleNavbar = new EventEmitter<void>();
  authService = inject(AuthService);

  onToggle() {
    this.toggleNavbar.emit();
  }

  logout() {
    this.authService.logout()
      .subscribe(
        () => {
          this.router.navigate([NavigationItems.login.navigationUrl]).then(_ => {
          });
        }
      )
  }
}
