import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class QuizService {
    constructor(private http: HttpClient) {}

    getQuizResult(id: string): Observable<QuizResult> {
        return this.http.get<QuizResult>(`/api/quiz/${id}`);
    }
}

export interface QuizResult {
    id: string;
    quizId: string;
    userId: string;
    data: QuizAnswer[];
}

export interface QuizAnswer {
    question: {
        text: string;
        type: string;
        data: string;
    };
    answer: {
        text: string;
        data: string;
    };
    score: {
        score: number;
        maxScore: number;
    };
}
