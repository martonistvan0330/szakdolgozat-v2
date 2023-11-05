import { AfterViewInit, Component, inject, OnInit } from '@angular/core';
import { NavigationItems } from "../../core-module";
import { CourseCard, Role } from "../../shared-module";
import { CourseService } from "../services/course.service";
import { catchError, of } from "rxjs";
import { AuthService } from "../../services";

@Component({
  selector: 'hwm-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.scss']
})
export class CourseListComponent implements OnInit, AfterViewInit {
  private courseService = inject(CourseService);
  private authService = inject(AuthService);
  protected readonly NavigationItems = NavigationItems;
  courses: CourseCard[] | null = null;
  isTeacher = false;
  isLoadingResults = true;

  ngOnInit() {
    this.authService.hasRole([Role.TEACHER])
      .subscribe(isTeacher => {
        this.isTeacher = isTeacher;
      });
  }

  ngAfterViewInit() {
    this.courseService.getCourseCards()
      .pipe(
        catchError(() => of(null))
      )
      .subscribe(courseCards => {
        this.isLoadingResults = false;
        this.courses = courseCards;
      });
  }
}
