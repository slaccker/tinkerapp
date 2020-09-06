import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { NewslistComponent } from './newslist/newslist.component';
import { NewsSourceServiceService } from './news-source-service.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { Ng2SearchPipeModule } from 'ng2-search-filter';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    NewslistComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgxPaginationModule,
    Ng2SearchPipeModule,
    RouterModule.forRoot([
      //{ path: '', component: HomeComponent, pathMatch: 'full' },

      { path: '', component: NewslistComponent, pathMatch: 'full' },
      { path: 'newslist', component: NewslistComponent },
    ]),
  ],
  providers: [NewsSourceServiceService],
  bootstrap: [AppComponent],
})
export class AppModule {}
