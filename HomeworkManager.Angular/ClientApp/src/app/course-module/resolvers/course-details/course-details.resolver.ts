import { ResolveFn } from '@angular/router';
import { inject } from "@angular/core";
import { CourseService } from "../../services/course.service";
import { CourseModel } from "../../../shared-module";
import { GroupService } from "../../services/group.service";

export const courseDetailsResolver: ResolveFn<CourseModel | null> = (route, _state) => {
  const courseService = inject(CourseService);
  const groupService = inject(GroupService);

  const courseId = route.paramMap.get('courseId');

  if (!courseId) {
    return null;
  }

  groupService.courseId = parseInt(courseId);
  return courseService.getCourse(parseInt(courseId));
};
