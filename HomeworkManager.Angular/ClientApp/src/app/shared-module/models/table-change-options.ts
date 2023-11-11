import { PageableOptions } from "./pageable-options";

export class TableChangeOptions<E> {
  constructor(
    public pageableOptions: PageableOptions,
    public extras: E | null = null
  ) {
  }
}