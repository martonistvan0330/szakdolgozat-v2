import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { NavigationItems } from "../../core-module";
import { CourseModel } from "../../shared-module";
import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";
import { BreakpointObserver, Breakpoints } from "@angular/cdk/layout";
import { map, shareReplay } from "rxjs/operators";
import { NavMenuComponent } from "../../control-module";

@Component({
  selector: 'hwm-course-details',
  templateUrl: './course-details.component.html',
  styleUrls: ['./course-details.component.scss']
})
export class CourseDetailsComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private breakpointObserver = inject(BreakpointObserver);
  protected readonly NavigationItems = NavigationItems;
  @ViewChild('groupList') groupList!: NavMenuComponent;
  course: CourseModel | null = null;
  isMedium$: Observable<boolean> = this.breakpointObserver.observe([Breakpoints.XSmall, Breakpoints.Small, Breakpoints.Medium])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  ngOnInit() {
    this.activatedRoute.data
      .subscribe(({ course }) => {
        const courseModel = course as CourseModel;
        this.course = courseModel;
      });
  }

  async toggleNavbar() {
    await this.groupList.toggle();
  }
}
