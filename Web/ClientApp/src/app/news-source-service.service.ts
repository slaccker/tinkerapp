import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NewsStory } from './models/newsstory.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NewsSourceServiceService {
  private _httpClient: HttpClient;
  private _baseUrl: string;

  constructor(
    @Inject(HttpClient) private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this._httpClient = http;
    this._baseUrl = baseUrl;
  }

  getBaseUrl() {
    return this._baseUrl;
  }

  getData(): Observable<NewsStory[]> {
    var data: any;

    debugger;
    data = this._httpClient.get<NewsStory[]>(this._baseUrl + 'newssource');
    debugger;

    return data;
  }
}
