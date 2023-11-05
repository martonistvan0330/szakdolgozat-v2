import { GroupModel } from "../group/group-model";

export class CourseModel {
  courseId!: number
  name!: string
  description: string | null = null
  groups: GroupModel[] = []
}