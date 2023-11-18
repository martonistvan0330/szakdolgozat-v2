import { ResolveFn } from '@angular/router';
import { inject } from "@angular/core";
import { AssignmentModel } from "../../../shared-module";
import { AssignmentService } from "../../services/assignment.service";

export const assignmentDetailsResolver: ResolveFn<AssignmentModel | null> = (route, state) => {
  const assignmentService = inject(AssignmentService);

  const assignmentId = route.paramMap.get('assignmentId');

  if (!assignmentId) {
    return null;
  }

  return assignmentService.getAssignment(parseInt(assignmentId));
};
