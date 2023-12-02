import { Component, Input } from '@angular/core';

@Component({
  selector: 'hwm-presentation',
  templateUrl: './presentation.component.html',
  styleUrls: ['./presentation.component.scss']
})
export class PresentationComponent {
  @Input() assignmentId!: number;
  @Input() isTeacher: boolean = false;
  isEditing: boolean = false;

  onAddClick() {
    this.isEditing = true;
  }

  onBack() {
    this.isEditing = false
  }
}
