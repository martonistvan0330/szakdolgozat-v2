export enum Errors {
  NoError = 0,

  Required = 1,
  RequiredTrue = 2,
  Min = 3,
  Max = 4,
  MinLength = 5,
  MaxLength = 6,
  Email = 7,
  Pattern = 8,

  ContainsLowerCase = 101,
  ContainsUpperCase = 102,
  ContainsDigit = 103,
  Equal = 104,

  Unique = 201
}
