import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NavigationItems } from "../../core-module";
import { catchError, merge, Observable, of, startWith } from "rxjs";
import { ColumnDefinition, Pageable, PageableOptions, PageData, SortOptions } from "../../shared-module";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { map } from "rxjs/operators";
import { FormControl, FormGroup } from "@angular/forms";

@Component({
  selector: 'hwm-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class TableComponent<T> implements OnInit, AfterViewInit {
  protected readonly NavigationItems = NavigationItems;
  @Input() title: string = '';
  @Input() columnDefs: ColumnDefinition[] = [];
  @Input() dataSource!: Observable<Pageable<T>> | Observable<T[]>;
  @Input() filterable = true;
  @Input() pageable = true;
  @Input() pageSize = 25;
  @Input() pageSizeOptions = [10, 25, 50];
  @Input() clickableRows = false;
  @Input() removableRows = false;
  @Input() removeIcon = 'remove';
  @Output() rowClick = new EventEmitter<T>();
  @Output() rowRemove = new EventEmitter<T>();
  @Output() optionsSetup = new EventEmitter<Observable<PageableOptions>>();
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  dataSource$: Observable<T[]> = of([]);
  displayedColumns!: string[];
  searchForm!: FormGroup;
  searchClicked = new EventEmitter<void>();
  resultsLength = 0;
  isLoadingResults = true;

  get search() {
    return this.searchForm.get('search')!!;
  }

  ngOnInit() {
    this.displayedColumns = this.columnDefs.map(column => column.fieldName);
    this.displayedColumns.push('actions');

    this.searchForm = new FormGroup({
      'search': new FormControl('')
    });
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    const pageableOptionsObservable =
      merge(this.sort.sortChange, this.paginator.page, this.searchClicked)
        .pipe(
          startWith({}),
          map(() => {
            this.isLoadingResults = true;

            const pageableOptions = new PageableOptions();

            const pageData = new PageData();
            pageData.pageIndex = this.paginator.pageIndex;
            pageData.pageSize = this.paginator.pageSize;
            pageableOptions.pageData = pageData;

            const sortOptions = new SortOptions();
            sortOptions.sort = this.sort.active;
            sortOptions.sortDirection = this.sort.direction;
            pageableOptions.sortOptions = sortOptions;

            pageableOptions.searchText = this.search.value;

            return pageableOptions;
          })
        );

    this.optionsSetup.emit(pageableOptionsObservable);

    if (this.pageable) {
      const castDataSource = this.dataSource as Observable<Pageable<T>>;

      this.dataSource$ =
        castDataSource
          .pipe(
            catchError(() => of(null)),
            map(data => {
              this.isLoadingResults = false;

              if (data === null) {
                return [];
              }

              this.resultsLength = data.totalCount;
              return data.items;
            })
          );
    } else {
      const castDataSource = this.dataSource as Observable<T[]>;

      this.dataSource$ =
        castDataSource
          .pipe(
            catchError(() => of(null)),
            map(data => {
              this.isLoadingResults = false;

              if (data === null) {
                return [];
              }

              return data;
            })
          );
    }
  }

  onRowClick(row: T) {
    if (this.clickableRows) {
      this.rowClick.emit(row);
    }
  }

  onRemoveClick(row: T) {
    if (this.removableRows) {
      this.rowRemove.emit(row);
    }
  }
}
