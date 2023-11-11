import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import {
  GroupListRow,
  GroupModel,
  NewGroup,
  Pageable,
  PageableOptions,
  UpdateGroup,
  UserListRow
} from "../../shared-module";
import { delay, Subject, tap } from "rxjs";

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

  getTeachers(groupName: string, options: PageableOptions) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Teacher'
      + '?pageData.pageIndex=' + options.pageData?.pageIndex
      + '&pageData.pageSize=' + options.pageData?.pageSize
      + '&sortOptions.sort=' + options.sortOptions?.sort
      + '&sortOptions.sortDirection=' + options.sortOptions?.sortDirection
      + '&searchText=' + options.searchText;

    return this.authApiClient.get<Pageable<UserListRow>>(requestUrl).pipe(delay(1000));
  }

  getStudents(groupName: string, options: PageableOptions) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Student'
      + '?pageData.pageIndex=' + options.pageData?.pageIndex
      + '&pageData.pageSize=' + options.pageData?.pageSize
      + '&sortOptions.sort=' + options.sortOptions?.sort
      + '&sortOptions.sortDirection=' + options.sortOptions?.sortDirection
      + '&searchText=' + options.searchText;

    return this.authApiClient.get<Pageable<UserListRow>>(requestUrl).pipe(delay(1000));
  }

  getAddableTeachers(groupName: string) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Teacher/Addable';

    return this.authApiClient.get<UserListRow[]>(requestUrl).pipe(delay(1000));
  }

  getAddableStudents(groupName: string) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Student/Addable';

    return this.authApiClient.get<UserListRow[]>(requestUrl).pipe(delay(1000));
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

  addTeachers(groupName: string, teacherIds: string[]) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Teacher/Add';

    return this.authApiClient.post<void>(requestUrl, teacherIds).pipe(delay(1000));
  }

  addStudents(groupName: string, studentIds: string[]) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Student/Add';

    return this.authApiClient.post<void>(requestUrl, studentIds).pipe(delay(1000));
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
