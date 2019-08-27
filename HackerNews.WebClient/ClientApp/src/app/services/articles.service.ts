import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ItemResults, HackerNews } from '../models/hacker-news.model';

@Injectable()
export class ArticlesService {
    private baseUrl: string;
    private http: HttpClient;
    constructor(private https: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.http = https;
        this.baseUrl = baseUrl;
    }

    findArticleById(articleId: number): Observable<HackerNews> {
        return this.http.get<HackerNews>(this.baseUrl + 'api/HackerNews/HackerNewsArticles/${articleId}');
    }

    findArticles(
        pageNumber = 0, pageSize = 25, filter = null) {

        return this.http.get<ItemResults>(this.baseUrl + 'api/HackerNews/HackerNewsArticles', {
            params: new HttpParams()
                .set('pageNumber', pageNumber.toString())
                .set('pageSize', pageSize.toString())
                .set('filter', (filter == null) ? '' : filter)
        });
    }
}
