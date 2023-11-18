import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import {
  AssignmentListRow,
  AssignmentListRowWithDate,
  GroupListRow,
  GroupModel,
  NewGroup,
  Pageable,
  PageableOptions,
  UpdateGroup,
  UserListRow
} from "../../shared-module";
import { delay, distinctUntilChanged, Subject, tap } from "rxjs";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private authApiClient = inject(AuthorizedApiClientService);
  private groupAdded = new Subject<void>();
  private groupUpdated = new Subject<void>();
  private groupChanged = new Subject<string>();
  private teacherAdded = new Subject<string>();
  private teacherRemoved = new Subject<string>();
  private studentAdded = new Subject<string>();
  private studentRemoved = new Subject<string>();
  groupAdded$ = this.groupAdded.asObservable();
  groupUpdated$ = this.groupUpdated.asObservable();
  groupChanged$ = this.groupChanged.asObservable().pipe(distinctUntilChanged());
  teacherAdded$ = this.teacherAdded.asObservable();
  teacherRemoved$ = this.teacherRemoved.asObservable();
  studentAdded$ = this.studentAdded.asObservable();
  studentRemoved$ = this.studentRemoved.asObservable();

  private _courseId!: number;

  get courseId() {
    return this._courseId;
  }

  set courseId(value: number) {
    this._courseId = value;
  }

  groupChange(groupName: string) {
    this.groupChanged.next(groupName);
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

  getAssignments(groupName: string, options: PageableOptions) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Assignment'
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

    return this.authApiClient.post<void>(requestUrl, teacherIds).pipe(
      tap(_groupId => {
        this.teacherAdded.next(groupName);
      }),
      delay(1000)
    );
  }

  addStudents(groupName: string, studentIds: string[]) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Student/Add';

    return this.authApiClient.post<void>(requestUrl, studentIds).pipe(
      tap(_groupId => {
        this.studentAdded.next(groupName);
      }),
      delay(1000)
    );
  }

  removeTeacher(groupName: string, teacherId: string) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Teacher/' + teacherId + '/Remove';

    return this.authApiClient.delete<void>(requestUrl).pipe(
      tap(_groupId => {
        this.teacherRemoved.next(groupName);
      }),
    );
  }

  removeStudent(groupName: string, studentId: string) {
    const requestUrl = 'Course/' + this._courseId + '/Group/' + groupName + '/Student/' + studentId + '/Remove';

    return this.authApiClient.delete<void>(requestUrl).pipe(
      tap(_groupId => {
        this.studentRemoved.next(groupName);
      }),
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
