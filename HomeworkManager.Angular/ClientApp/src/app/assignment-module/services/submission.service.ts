import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import {
  FileSubmissionModel,
  Pageable,
  PageableOptions,
  SubmissionListRow,
  SubmissionListRowWithDate,
  TextSubmissionModel,
  UpdatedTextSubmission
} from "../../shared-module";
import { delay } from "rxjs";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class SubmissionService {
  private authApiClient = inject(AuthorizedApiClientService);

  getTextSubmission(assignmentId: number, userId: string | null) {
    if (userId) {
      return this.authApiClient.get<TextSubmissionModel>('Submission/Text/' + assignmentId + '?userId=' + userId);
    }

    return this.authApiClient.get<TextSubmissionModel | null>('Submission/Text/' + assignmentId);
  }

  getFileSubmission(assignmentId: number, userId: string | null) {
    if (userId) {
      return this.authApiClient.get<FileSubmissionModel>('Submission/File/' + assignmentId + '?userId=' + userId);
    }

    return this.authApiClient.get<FileSubmissionModel | null>('Submission/File/' + assignmentId);
  }

  downloadFileSubmission(assignmentId: number) {
    return this.authApiClient.download('Submission/File/' + assignmentId + '/Download');
  }

  getSubmissions(assignmentId: number, options: PageableOptions) {
    const requestUrl = 'Submission/' + assignmentId
      + '?pageData.pageIndex=' + options.pageData?.pageIndex
      + '&pageData.pageSize=' + options.pageData?.pageSize
      + '&sortOptions.sort=' + options.sortOptions?.sort
      + '&sortOptions.sortDirection=' + options.sortOptions?.sortDirection
      + '&searchText=' + options.searchText;

    return this.authApiClient.get<Pageable<SubmissionListRow>>(requestUrl)
      .pipe(
        delay(1000),
        map(submissionListRows => {
          const submissionListRowsWithDate = submissionListRows.items.map(submissionListRow => {
            const submissionListRowWithDate = new SubmissionListRowWithDate();

            submissionListRowWithDate.submissionId = submissionListRow.submissionId;
            submissionListRowWithDate.studentId = submissionListRow.studentId;
            submissionListRowWithDate.studentName = submissionListRow.studentName;
            submissionListRowWithDate.submittedAt = new Date(submissionListRow.submittedAt.split('+')[0]);

            return submissionListRowWithDate;
          });

          return {
            items: submissionListRowsWithDate,
            totalCount: submissionListRows.totalCount
          } as Pageable<SubmissionListRowWithDate>;
        }));
  }

  updateTextSubmission(updatedTextSubmission: UpdatedTextSubmission) {
    return this.authApiClient.post<number>('Submission/Text', updatedTextSubmission);
  }

  uploadFileSubmission(assignmentId: number, file: File) {
    const formData = new FormData();

    formData.append('submission', file);

    return this.authApiClient.post<number>('Submission/File/' + assignmentId, formData);
  }

  submit(assignmentId: number) {
    return this.authApiClient.patch('Submission/' + assignmentId, {});
  }
}
