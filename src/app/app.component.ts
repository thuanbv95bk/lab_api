import { Component, OnDestroy, OnInit } from '@angular/core';
import { AppGlobals } from '../common/app-global';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'FromLogin';
  defaultLang: string = 'vi';
  constructor(public translate: TranslateService) {}
  ngOnInit(): void {
    // localStorage.removeItem('language');
    this.initTranslate();
  }

  ngOnDestroy(): void {}

  initTranslate() {
    const savedLang = AppGlobals.getLang().toString();
    console.log(savedLang);

    if (!savedLang || savedLang == '') {
      this.translate.setDefaultLang(this.defaultLang);
      AppGlobals.setLang(this.defaultLang);
      return;
    }
    this.translate.setDefaultLang(savedLang);
  }
}
