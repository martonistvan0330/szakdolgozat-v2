export class EmailConfirmationRequest {
  token: string

  constructor(token: string) {
    this.token = token;
  }
}