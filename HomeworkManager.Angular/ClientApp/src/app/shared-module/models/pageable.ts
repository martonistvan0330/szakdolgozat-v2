export interface Pageable<T> {
  items: T[];
  totalCount: number;
}