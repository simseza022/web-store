import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FogortPasswordPageComponent } from './fogort-password-page.component';

describe('FogortPasswordPageComponent', () => {
  let component: FogortPasswordPageComponent;
  let fixture: ComponentFixture<FogortPasswordPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FogortPasswordPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FogortPasswordPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
