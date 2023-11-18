import { AssignmentTypeId } from "./assignment-type-id";

export class AssignmentModel {
  assignmentId!: number;
  name!: string;
  description: string | null = null;
  deadline!: string;
  presentationRequired: boolean = false;
  isDraft: boolean = true;
  assignmentTypeId: AssignmentTypeId | null = null;
  assignmentTypeName: string | null = null;
  courseId!: number
  courseName!: string
  groupId!: number
  groupName!: string
}