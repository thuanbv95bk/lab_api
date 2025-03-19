import { Injectable } from '@angular/core';
import { ActiveToast, ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  constructor(protected toastr: ToastrService) {}

  showSuccess(message: string, title?: string) {
    this.toastr.success(message, title, { timeOut: 3000 });
  }

  showError(message: string, title?: string) {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const toast: ActiveToast<any> = this.toastr.error(message, title, {
      timeOut: 5000,
    });
    return toast;
  }

  showInfo(message: string, title?: string) {
    this.toastr.info(message, title, { timeOut: 6000 });
  }

  showWarning(message: string, title?: string) {
    this.toastr.warning(message, title, { timeOut: 6000 });
  }
}
