import { ResolveFn } from '@angular/router';
import { UserModel } from "../../shared-module";
import { inject } from "@angular/core";
import { UserService } from "../services/user.service";

export const userResolver: ResolveFn<UserModel | null> = (route, state) => {
  const userService = inject(UserService);

  const userId = route.paramMap.get('id');

  if (!userId) {
    return null;
  }

  return userService.getUser(userId);
};
