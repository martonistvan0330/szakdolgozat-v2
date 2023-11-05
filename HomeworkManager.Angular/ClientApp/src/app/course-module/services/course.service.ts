import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import { CourseCard, CourseModel, NewCourse } from "../../shared-module";
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

  getCourse(courseId: number) {
    return this.authApiClient.get<CourseModel>('Course/' + courseId);
  }

  createCourse(newCourse: NewCourse) {
    return this.authApiClient.post<number>('Course', newCourse);
  }

  updateCourse(courseId: number, updatedCourse: UpdateCourse) {
    return this.authApiClient.put<void>('Course/' + courseId, updatedCourse);
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
