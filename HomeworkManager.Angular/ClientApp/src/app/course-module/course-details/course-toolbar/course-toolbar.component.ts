import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { NavigationItems } from "../../../core-module";
import { CourseModel, Role } from "../../../shared-module";
import { CourseService } from "../../services/course.service";
import { AuthService } from "../../../services";

@Component({
  selector: 'hwm-course-toolbar',
  templateUrl: './course-toolbar.component.html',
  styleUrls: ['./course-toolbar.component.scss']
})
export class CourseToolbarComponent implements OnInit {
  private courseService = inject(CourseService);
  private authService = inject(AuthService);
  protected readonly NavigationItems = NavigationItems;
  @Input() course!: CourseModel;
  @Input() isMobile: boolean | null = false;
  @Output() toggleNavbar = new EventEmitter<void>();
  isAdministrator = false
  isCreator = false;
  isTeacher = false;

  ngOnInit() {
    this.authService.hasRole([Role.ADMINISTRATOR])
      .subscribe(isAdmin => {
        this.isAdministrator = isAdmin;
      });

    this.courseService.isCreator(this.course.courseId)
      .subscribe({
        next: isCreator => {
          this.isCreator = isCreator;
        }
      });

    this.courseService.isTeacher(this.course.courseId)
      .subscribe({
        next: isTeacher => {
          this.isTeacher = isTeacher;
        }
      });
  }

  onToggle() {
    this.toggleNavbar.emit();
  }
}
