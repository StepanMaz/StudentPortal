import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class RealtimeNotificationsService {
    private hubConnection: HubConnection;
    private pendingNotifications = new Subject<NotificationDTO>();

    notifications$ = this.pendingNotifications.asObservable();

    constructor() {
        this.hubConnection = new HubConnectionBuilder().withUrl(environment.signalRUrl).build();

        this.hubConnection.start().then(
            () => console.log('Notifications WebSocket connection started.'),
            (e) => console.error('Error establishing notifications WebSocket connection:', e)
        );

        this.hubConnection.on('ReceiveNotification', (notification) => {
            this.pendingNotifications.next(notification);
        });

        this.hubConnection.on('MessageReceived', (message) => {
            console.log('New message:', message);
        });
    }
}
