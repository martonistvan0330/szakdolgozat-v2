import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import { CourseCard, CourseModel, NewCourse, UserListRow } from "../../shared-module";
import { delay } from "rxjs";
import { UpdateCourse } from "../../shared-module/models/course/update-course";

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  private authApiClient = inject(AuthorizedApiClientService);

  getCourseCards() {
    return this.authApiClient.get<CourseCard[]>('Course').pipe(delay(1000));
  }

  existsCourse(courseId: number) {
    return this.authApiClient.get<boolean>('Course/' + courseId + '/Exist');
  }

  getCourse(courseId: number) {
    return this.authApiClient.get<CourseModel | null>('Course/' + courseId);
  }

  getAddableTeachers(courseId: number) {
    return this.authApiClient.get<UserListRow[]>('Course/' + courseId + '/Teacher/Addable');
  }

  getAddableStudents(courseId: number) {
    return this.authApiClient.get<UserListRow[]>('Course/' + courseId + '/Student/Addable');
  }

  createCourse(newCourse: NewCourse) {
    return this.authApiClient.post<number>('Course', newCourse);
  }

  updateCourse(courseId: number, updatedCourse: UpdateCourse) {
    return this.authApiClient.put<void>('Course/' + courseId, updatedCourse);
  }

  addTeachers(courseId: number, teacherIds: string[]) {
    return this.authApiClient.post<void>('Course/' + courseId + '/Teacher/Add', teacherIds);
  }

  addStudents(courseId: number, studentIds: string[]) {
    return this.authApiClient.post<void>('Course/' + courseId + '/Student/Add', studentIds);
  }

  isInCourse(courseId: number) {
    return this.authApiClient.get<boolean>('Course/' + courseId + '/IsInCourse');
  }

  isCreator(courseId: number) {
    return this.authApiClient.get<boolean>('Course/' + courseId + '/IsCreator');
  }

  isTeacher(courseId: number) {
    return this.authApiClient.get<boolean>('Course/' + courseId + '/IsTeacher');
  }

  nameAvailable(name: string) {
    return this.authApiClient.get<boolean>('Course/NameAvailable?name=' + name)
  }
}
