import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { AuthService } from "../../../core-module";

@Component({
  selector: 'hwm-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent {
  @Input() isMobile: boolean | null = false;
  @Output() toggleNavbar = new EventEmitter<void>();
  authService = inject(AuthService);

  onClick() {
    this.toggleNavbar.emit();
  }
}
