import { AssignmentTypeId } from "./assignment-type-id";

export class UpdatedAssignment {
  assignmentId!: number;
  name!: string;
  description!: string;
  deadline!: string;
  presentationRequired!: boolean;
  assignmentTypeId!: AssignmentTypeId;
}