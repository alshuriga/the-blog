import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentaryFormComponent } from './commentary-form.component';

describe('CommentaryFormComponent', () => {
  let component: CommentaryFormComponent;
  let fixture: ComponentFixture<CommentaryFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommentaryFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommentaryFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
