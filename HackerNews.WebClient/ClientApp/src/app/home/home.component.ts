import { Component, ViewChild, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { PageEvent, MatPaginator } from '@angular/material/paginator';
import { ArticlesDataSource } from './articles.datasorce';
import { ArticlesService } from '../services/articles.service';
import { HackerNewsComponent } from '../modal/hacker-news.component';
import { tap } from 'rxjs/operators';
import { HackerNews } from '../models/hacker-news.model';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, AfterViewInit {
  public articles: HackerNews[];
  public article: HackerNews;
  public displayedColumns: string[] = ['title', 'url'];
  public changeDetectorRefs: ChangeDetectorRef;
  private selectedRowIndex = -1;

  dataSource: ArticlesDataSource;
  length = 100;
  pageSize = 25;
  value = 50;
  doneLoading: boolean;
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  row: any;
  public filterString: string;

  set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.dataSource.paginator = this.paginator;
  }
  pageSizeOptions: number[] = [25, 50, 75, 100];
  // MatPaginator Output
  pageEvent: PageEvent;
  service: ArticlesService;
  applyFilter(filterValue: string) {
    this.filterString = filterValue.trim().toLowerCase();
    this.loadPage();
  }
  constructor(private dialog: MatDialog, service: ArticlesService, changeDetect: ChangeDetectorRef) {
    this.service = service;
    this.changeDetectorRefs = changeDetect;
  }

  ngOnInit() {
    this.doneLoading = false;
    this.dataSource = new ArticlesDataSource(this.service);

    this.dataSource.loadArticles(null, 0, 25, null);
    this.dataSource.loading$.subscribe((x: boolean) => {
      this.length = this.dataSource.totalItems;
      this.doneLoading = x;
    }
    );
  }

  ngAfterViewInit() {
    this.paginator.page.pipe(
      tap(() => this.loadPage())
    )
      .subscribe();
  }
  openModal(row: HackerNews) {
    this.dialog.open(HackerNewsComponent, { data: row });
  }
  highlight(row: { id: number; }) {
    this.selectedRowIndex = row.id;
  }
  loadPage() {
    this.dataSource.loadArticles(
      null,
      this.paginator.pageIndex,
      this.paginator.pageSize,
      this.filterString);
  }
}

