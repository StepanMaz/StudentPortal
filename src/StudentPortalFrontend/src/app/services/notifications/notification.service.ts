import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class NotificationService {
    constructor(private httpClient: HttpClient) {}

    getNotifications(): Observable<NotificationDTO> {
        return this.httpClient.get<NotificationDTO>('/notifications');
    }
}
