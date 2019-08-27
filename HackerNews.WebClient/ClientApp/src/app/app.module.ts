import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import {MatFormFieldModule} from '@angular/material/form-field';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import {HackerNewsComponent} from './modal/hacker-news.component';
import { HomeComponent } from './home/home.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule} from '@angular/material/table';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { MatDialogModule} from '@angular/material/dialog';
import {ArticlesService} from './services/articles.service';
import { SafeHtmlPipe } from './pipes/pipes.safehtml';
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    HackerNewsComponent,
    SafeHtmlPipe
  ],
  entryComponents: [HackerNewsComponent],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatTableModule,
    MatDialogModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' }
    ])
  ],
  exports: [HackerNewsComponent],
  providers: [ArticlesService],
  bootstrap: [AppComponent]
})
export class AppModule { }
