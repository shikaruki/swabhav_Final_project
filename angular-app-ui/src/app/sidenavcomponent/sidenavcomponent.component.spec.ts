import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidenavcomponentComponent } from './sidenavcomponent.component';

describe('SidenavcomponentComponent', () => {
  let component: SidenavcomponentComponent;
  let fixture: ComponentFixture<SidenavcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SidenavcomponentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidenavcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
