import { ComponentFixture, TestBed } from '@angular/core/testing';
import { beforeEach, describe, expect, it } from '@jest/globals';
import { CampusFeedComponent } from './campus-feed.component';

describe('CampusFeedComponent', () => {
  let component: CampusFeedComponent;
  let fixture: ComponentFixture<CampusFeedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CampusFeedComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CampusFeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
