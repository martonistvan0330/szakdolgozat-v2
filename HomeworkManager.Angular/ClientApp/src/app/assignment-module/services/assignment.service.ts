import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import { AssignmentModel, AssignmentType, NewAssignment } from "../../shared-module";
import { UpdatedAssignment } from "../../shared-module/models/assignment/updated-assignment";

@Injectable({
  providedIn: 'root'
})
export class AssignmentService {
  private authApiClient = inject(AuthorizedApiClientService);

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

  getTypes() {
    return this.authApiClient.get<AssignmentType[]>('Assignment/Types');
  }
}
