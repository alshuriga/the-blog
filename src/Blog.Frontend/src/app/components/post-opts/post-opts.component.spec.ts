import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostOptsComponent } from './post-opts.component';

describe('PostOptsComponent', () => {
  let component: PostOptsComponent;
  let fixture: ComponentFixture<PostOptsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PostOptsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostOptsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
