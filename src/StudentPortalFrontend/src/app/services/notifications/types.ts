type NotificationDTO = {
    id: string;
    title: string;
    message: string;
    read: boolean;
    acknowledged: boolean;
    createdAt: string;
    readAt?: string;
    metadata?: string;
};
