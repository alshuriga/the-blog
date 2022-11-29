import { Component, OnInit, SimpleChanges, Input, OnChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.css']
})

export class PaginatorComponent implements OnInit, OnChanges {
  @Input() currentPage: number;
  @Input() pageCount: number;
  @Output() toPage = new EventEmitter<number>();
  buttons: PageButton[] = [];

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {

    this.buttons = this.generatePageButtons();
    
  }

  ngOnChanges(changes: SimpleChanges) {

    this.pageCount = changes['pageCount'] ? changes['pageCount'].currentValue : this.pageCount;
    console.log(changes['pageCount'])
    this.currentPage = changes['currentPage'] ? changes['currentPage'].currentValue : this.currentPage;
    this.buttons = this.generatePageButtons();
  }
  

  pageButtonClick(event: Event) {
    let id = (event.target as Element).id;
    this.toPage.emit(Number.parseInt(id));
  }

  private generatePageButtons(): PageButton[] {

    let buttons: PageButton[] = [];
    if(this.pageCount <= 1) return buttons;
    let from = this.currentPage <= 0 ? 0 : this.currentPage - 1;
    let to = this.currentPage >= this.pageCount - 1 ? this.currentPage : this.currentPage + 1;
    buttons.push({
      text: "First",
      page: 0,
      disabled: this.currentPage === 0,
      active: false
    });

    for (let i = from; i <= to; i++) {
      buttons.push({
        text: (i + 1).toString(),
        page: i,
        disabled: false,
        active: i === this.currentPage
      });
    }

    buttons.push({
      text: "Last",
      page: this.pageCount - 1,
      disabled: this.currentPage === this.pageCount - 1 ,
      active: false
    });

    return buttons;
  }

}

type PageButton = {
  text: string;
  page: number;
  disabled: boolean;
  active: boolean;
}
