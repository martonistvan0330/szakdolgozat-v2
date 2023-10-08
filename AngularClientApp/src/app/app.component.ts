import {Component, isDevMode, OnInit} from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'AngularClientApp';

  ngOnInit() {
    if (!isDevMode()) {
      this.title = "AngularClientAppProd"
    }
  }
}
