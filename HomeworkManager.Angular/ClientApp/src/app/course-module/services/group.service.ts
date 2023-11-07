import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import { GroupListRow, GroupModel, NewGroup, UpdateGroup } from "../../shared-module";
import { Subject, tap } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private authApiClient = inject(AuthorizedApiClientService);
  private groupAdded = new Subject<void>();
  private groupUpdated = new Subject<void>();
  groupAdded$ = this.groupAdded.asObservable();
  groupUpdated$ = this.groupUpdated.asObservable();


  private _courseId!: number;

  set courseId(value: number) {
    this._courseId = value;
  }

  getGroups() {
    return this.authApiClient.get<GroupListRow[]>('Course/' + this._courseId + '/Group');
  }

  existsGroup(groupName: string) {
    return this.authApiClient.get<boolean>('Course/' + this._courseId + '/Group/' + groupName + '/Exist');
  }

  getGroup(groupName: string) {
    return this.authApiClient.get<GroupModel | null>('Course/' + this._courseId + '/Group/' + groupName);
  }

  createGroup(newGroup: NewGroup) {
    return this.authApiClient.post<number>('Course/' + this._courseId + '/Group', newGroup).pipe(
      tap(_groupId => {
        this.groupAdded.next();
      })
    );
  }

  updateGroup(groupName: string, updatedGroup: UpdateGroup) {
    return this.authApiClient.put<void>('Course/' + this._courseId + '/Group/' + groupName, updatedGroup)
      .pipe(
        tap(() => {
          this.groupUpdated.next();
        })
      );
  }

  isInGroup(groupName: string) {
    return this.authApiClient.get<boolean>('Course/' + this._courseId + '/Group/' + groupName + '/IsInGroup');
  }

  isCreator(groupName: string) {
    return this.authApiClient.get<boolean>('Course/' + this._courseId + '/Group/' + groupName + '/IsCreator');
  }

  isTeacher(groupName: string) {
    return this.authApiClient.get<boolean>('Course/' + this._courseId + '/Group/' + groupName + '/IsTeacher');
  }

  nameAvailable(name: string) {
    return this.authApiClient.get<boolean>('Course/' + this._courseId + '/Group/NameAvailable?name=' + name)
  }
}
