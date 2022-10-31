import { Injectable, Type } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServerValidationService {
  private serverErrorsData = new BehaviorSubject(null);
  public serverErrors$ = this.serverErrorsData.asObservable();

  constructor() { }

  addServerErrors(errors: any) 
  {
    this.serverErrorsData.next(errors);
  }

  removeServerErrors()
  {
    this.serverErrorsData.next(null);
  }

}


