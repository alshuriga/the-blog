import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostMinComponent } from './post-min.component';

describe('PostMinComponent', () => {
  let component: PostMinComponent;
  let fixture: ComponentFixture<PostMinComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PostMinComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostMinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
