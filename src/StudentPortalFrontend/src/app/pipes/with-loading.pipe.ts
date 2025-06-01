import { Pipe, PipeTransform } from '@angular/core';
import { catchError, isObservable, map, Observable, of, startWith } from 'rxjs';

type LoadingValue<T> = { loading: boolean; error?: any; value?: T };

@Pipe({
    name: 'withLoading',
    standalone: true,
})
export class WithLoadingPipe implements PipeTransform {
    transform<T>(val: Observable<T>): Observable<LoadingValue<T>> {
        return val.pipe(
            map((value) => ({ loading: false, value }) as const),
            startWith({ loading: true } as const),
            catchError((error) => of({ loading: false, error } as const)),
        );
    }
}
