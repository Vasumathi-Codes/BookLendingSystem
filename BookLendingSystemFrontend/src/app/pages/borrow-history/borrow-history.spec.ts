import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BorrowHistory } from './borrow-history';

describe('BorrowHistory', () => {
  let component: BorrowHistory;
  let fixture: ComponentFixture<BorrowHistory>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BorrowHistory]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BorrowHistory);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
