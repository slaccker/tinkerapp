import { TestBed } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { NewsSourceServiceService } from './news-source-service.service';
import {
  HttpTestingController,
  HttpClientTestingModule,
} from '@angular/common/http/testing';
import { NewsStory } from './models/newsstory.model';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { Ng2SearchPipeModule } from 'ng2-search-filter';

describe('NewsSourceServiceService', () => {
  let service: NewsSourceServiceService;
  let httpMock: HttpTestingController;
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        NgxPaginationModule,
        Ng2SearchPipeModule,
        FormsModule,
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
      providers: [
        NewsSourceServiceService,
        { provide: 'BASE_URL', useValue: 'https://localhost.mock/' },
      ],
    }).compileComponents();
    service = TestBed.get(NewsSourceServiceService);
    httpMock = TestBed.get(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should be able to GET NewsStory from API', (done) => {
    const testNewsStorys: NewsStory[] = [
      {
        id: 111,
        title: 'Story 111',
        url: 'https://title.story1.mock/mock-story-1.html',
        detail: 'This is a testing story',
      },
      {
        id: 222,
        title: 'Story 222',
        url: '',
        detail: 'This is a testing story without a URL',
      },
    ];
    service.getData().subscribe((newslist) => {
      expect(newslist.length).toBe(2);
      expect(newslist).toEqual(testNewsStorys);
      done();
    });
    const httpRequest = httpMock.expectOne('https://localhost.mock/newssource');
    expect(httpRequest.request.method).toBe('GET');
    httpRequest.flush(testNewsStorys);
  });
});
