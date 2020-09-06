import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Input,
  ChangeDetectorRef,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { NewsStory } from '../models/newsstory.model';
import { NewsSourceServiceService } from '../news-source-service.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-newslist',
  templateUrl: './newslist.component.html',
  styleUrls: ['./newslist.component.css'],
  providers: [NewsSourceServiceService],
})
export class NewslistComponent implements OnInit {
  private _newslist: Observable<NewsStory[]>;

  public selectedStory: NewsStory = null;

  constructor(
    private newsSourceService: NewsSourceServiceService,
    private cdRef: ChangeDetectorRef
  ) {
    // this.cdRef.detach();
  }

  ngOnInit() {
    this.selectedStory = null;
    this.getNews();
  }

  // ngOnChanges(changes: SimpleChanges): void {
  //   debugger;
  //   this.cdRef.detectChanges();
  // }

  public get newslist(): Observable<NewsStory[]> {
    return this._newslist;
  }

  @Input()
  public set newslist(value: Observable<NewsStory[]>) {
    if (value) {
      this._newslist = value;
      // this.cdRef.markForCheck();
      // debugger;
    }
  }

  getNews(): void {
    // debugger;
    this.newslist = this.newsSourceService.getData();
  }

  onSelect(newsStory: NewsStory): void {
    this.selectedStory = newsStory;
  }
}
