import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';

export const loggingInterceptor: HttpInterceptorFn = (req, next) => {
  console.log(`[HTTP] ${req.method} ${req.url}`);
  
  return next(req).pipe();
};
