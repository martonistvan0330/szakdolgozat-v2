export class ColumnDefinition {
  constructor(
    public readonly label: string,
    public readonly fieldName: string,
    public readonly isSorted: boolean = true) {
  }
}