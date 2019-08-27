import {CollectionViewer, DataSource} from '@angular/cdk/collections';
import {Observable, BehaviorSubject, of} from 'rxjs';
import {catchError, finalize} from 'rxjs/operators';
import { ArticlesService } from '../services/articles.service';
import { MatPaginator } from '@angular/material/paginator';
import { ItemResults, HackerNews } from '../models/hacker-news.model';



export class ArticlesDataSource implements DataSource<HackerNews> {

    private articlesSubject = new BehaviorSubject<HackerNews[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);
    public loading$ = this.loadingSubject.asObservable();
    private articlesService: ArticlesService;
    public filter: string;
    public data: HackerNews[];
    public paginator: MatPaginator;
    public totalItems: number;
    constructor(private service: ArticlesService) {
        this.articlesService = service;
        }

    loadArticles(articleId: number,
                pageIndex: number,
                pageSize: number,
                filter: any) {

        this.loadingSubject.next(true);

        this.articlesService.findArticles(pageIndex, pageSize, filter).pipe(
                catchError(() => of([])),
                finalize(() => {
                    console.log('Finsied Loading');
                    this.loadingSubject.next(false);
                })
            )
            .subscribe((res: ItemResults) => {
                this.totalItems = res.totalCount;
                this.articlesSubject.next(res.items);
            });

    }

    connect(collectionViewer: CollectionViewer): Observable<HackerNews[]> {
        console.log('Connecting data source');
        return this.articlesSubject.asObservable();
    }

    disconnect(collectionViewer: CollectionViewer): void {
        this.articlesSubject.complete();
        this.loadingSubject.complete();
    }
}
