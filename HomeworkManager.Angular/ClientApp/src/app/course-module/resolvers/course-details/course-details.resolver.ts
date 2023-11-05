import { ResolveFn } from '@angular/router';
import { inject } from "@angular/core";
import { CourseService } from "../../services/course.service";
import { CourseModel } from "../../../shared-module";

export const courseDetailsResolver: ResolveFn<CourseModel | null> = (route, _state) => {
  const courseService = inject(CourseService);

  const courseId = route.paramMap.get('courseId');

  if (!courseId) {
    return null;
  }

  return courseService.getCourse(parseInt(courseId));
};
