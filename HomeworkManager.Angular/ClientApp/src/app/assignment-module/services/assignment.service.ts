import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import {
  AssignmentListRow,
  AssignmentListRowWithDate,
  AssignmentModel,
  AssignmentType,
  AssignmentTypeId,
  NewAssignment,
  Pageable,
  PageableOptions
} from "../../shared-module";
import { UpdatedAssignment } from "../../shared-module/models/assignment/updated-assignment";
import { delay } from "rxjs";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AssignmentService {
  private authApiClient = inject(AuthorizedApiClientService);

  getAssignments(options: PageableOptions) {
    const requestUrl = 'Assignment'
      + '?pageData.pageIndex=' + options.pageData?.pageIndex
      + '&pageData.pageSize=' + options.pageData?.pageSize
      + '&sortOptions.sort=' + options.sortOptions?.sort
      + '&sortOptions.sortDirection=' + options.sortOptions?.sortDirection
      + '&searchText=' + options.searchText;

    return this.authApiClient.get<Pageable<AssignmentListRow>>(requestUrl)
      .pipe(
        delay(1000),
        map(assignmentListRows => {
          const assignmentListRowsWithDate = assignmentListRows.items.map(assignmentListRow => {
            const assignmentListRowWithDate = new AssignmentListRowWithDate();

            assignmentListRowWithDate.assignmentId = assignmentListRow.assignmentId;
            assignmentListRowWithDate.name = assignmentListRow.name;
            assignmentListRowWithDate.isDraft = assignmentListRow.isDraft;
            assignmentListRowWithDate.deadline = new Date(assignmentListRow.deadline.split('+')[0]);

            return assignmentListRowWithDate;
          });

          return {
            items: assignmentListRowsWithDate,
            totalCount: assignmentListRows.totalCount
          } as Pageable<AssignmentListRowWithDate>;
        }));
  }

  getAssignment(assignmentId: number) {
    return this.authApiClient.get<AssignmentModel>('Assignment/' + assignmentId);
  }

  createAssignment(newAssignment: NewAssignment) {
    return this.authApiClient.post<number>('Assignment', newAssignment);
  }

  updateAssignment(updatedAssignment: UpdatedAssignment) {
    return this.authApiClient.put<number>('Assignment/' + updatedAssignment.assignmentId, updatedAssignment);
  }

  publishAssignment(assignmentId: number) {
    return this.authApiClient.patch<number>('Assignment/Publish/' + assignmentId, {});
  }

  isInGroup(assignmentId: number) {
    return this.authApiClient.get<boolean>('Assignment/' + assignmentId + '/IsInGroup')
  }

  isTeacher(assignmentId: number) {
    return this.authApiClient.get<boolean>('Assignment/' + assignmentId + '/IsTeacher')
  }

  isCreator(assignmentId: number) {
    return this.authApiClient.get<boolean>('Assignment/' + assignmentId + '/IsCreator')
  }

  nameAvailable(name: string, courseId: number, groupName: string) {
    const requestUrl = 'Assignment/NameAvailable'
      + '?name=' + name
      + '&groupInfo.courseId=' + courseId
      + '&groupInfo.name=' + groupName

    return this.authApiClient.get<boolean>(requestUrl);
  }

  updatedNameAvailable(name: string, assignmentId: number) {
    const requestUrl = 'Assignment/NameAvailable/'
      + assignmentId
      + '?name=' + name

    return this.authApiClient.get<boolean>(requestUrl);
  }

  getAssignmentTypeId(assignmentId: number) {
    return this.authApiClient.get<AssignmentTypeId>('Assignment/' + assignmentId + '/Type')
  }

  getTypes() {
    return this.authApiClient.get<AssignmentType[]>('Assignment/Types');
  }
}
