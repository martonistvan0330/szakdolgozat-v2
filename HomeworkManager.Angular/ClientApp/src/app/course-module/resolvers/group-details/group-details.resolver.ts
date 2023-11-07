import { ResolveFn } from '@angular/router';
import { inject } from "@angular/core";
import { GroupService } from "../../services/group.service";
import { GroupModel } from "../../../shared-module";

export const groupDetailsResolver: ResolveFn<GroupModel | null> = (route, state) => {
  const groupService = inject(GroupService);

  const groupName = route.paramMap.get('groupName');

  if (!groupName) {
    return null;
  }

  return groupService.getGroup(groupName);
};
