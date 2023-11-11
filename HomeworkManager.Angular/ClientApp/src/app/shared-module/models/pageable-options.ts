import { PageData } from "./page-data";
import { SortOptions } from "./sort-options";

export class PageableOptions {
  pageData: PageData | null = null;
  sortOptions: SortOptions | null = null;
  searchText: string | null = null;
}