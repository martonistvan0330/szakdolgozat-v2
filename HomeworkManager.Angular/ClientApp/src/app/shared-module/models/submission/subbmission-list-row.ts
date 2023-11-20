export class SubmissionListRow {
  submissionId!: number;
  studentId!: number;
  studentName!: string;
  submittedAt!: string;
}

export class SubmissionListRowWithDate {
  submissionId!: number;
  studentId!: number;
  studentName!: string;
  submittedAt!: Date;
}