import { Component, inject, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from "@angular/forms";
import { ColumnDefinition } from "../../shared-module";
import { BehaviorSubject, Observable, startWith } from "rxjs";
import { map } from "rxjs/operators";
import { MatAutocompleteSelectedEvent } from "@angular/material/autocomplete";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
  selector: 'hwm-list-add-dialog',
  templateUrl: './list-add-dialog.component.html',
  styleUrls: ['./list-add-dialog.component.scss']
})
export class ListAddDialogComponent<T> implements OnInit {
  private dialogRef = inject(MatDialogRef<ListAddDialogComponent<T>>);
  private items: T[] = [];
  @Input() title = ''
  @Input() compareFn: ((a: T, b: T) => number) | null = null;
  @Input() displayFn: ((value: any) => string) | null = null;
  @Input() dataSource!: Observable<T[]>;
  @Input() columnDefs: ColumnDefinition[] = [];
  filteredItems!: Observable<T[]>;
  selectedItems: T[] = [];
  private selectedItemsSubject = new BehaviorSubject(this.selectedItems);
  selectedItems$ = this.selectedItemsSubject.asObservable();
  searchForm!: FormGroup;

  get searchText() {
    return this.searchForm.get('searchText')!!;
  }

  ngOnInit() {
    this.setupForm();

    this.dataSource.subscribe(items => {
      this.items = items;
    });
  }

  onOptionSelected(event: MatAutocompleteSelectedEvent) {
    this.selectedItems.push(event.option.value as T);

    if (this.compareFn) {
      this.selectedItems.sort(this.compareFn)
    }

    this.updateTable()

    this.searchText.reset('');
  }

  onRemoveClick(item: T) {
    const index = this.selectedItems.indexOf(item);
    this.selectedItems.splice(index, 1);

    this.updateTable()
  }

  onCancelClick() {
    this.dialogRef.close();
  }

  private setupForm() {
    this.searchForm = new FormGroup({
      searchText: new FormControl('')
    });

    this.filteredItems = this.searchText.valueChanges
      .pipe(
        startWith(''),
        map(value => this.filter(value || ''))
      );
  }

  private filter(value: string) {
    const filterValue = value.toString().toLowerCase();

    return this.items.filter(item => {
      if (this.selectedItems.includes(item)) {
        return false;
      }

      for (const columnDefinition of this.columnDefs) {
        // @ts-ignore
        if (item[columnDefinition.fieldName].toString().toLowerCase().includes(filterValue)) {
          return true;
        }
      }

      return false;
    })
  }

  private updateTable() {
    this.selectedItemsSubject.next(this.selectedItems);
  }
}
