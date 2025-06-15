import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { filter, from, map, mergeMap, Observable, toArray } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class QuizService {
    constructor(private http: HttpClient) {}

    publish(quiz: QuizResult) {
        return this.http.post(`/api/quiz`, quiz);
    }

    getQuizResult(id: string): Observable<QuizResult> {
        return this.http.get<QuizResult>(`/api/quiz/${id}`);
    }

    getMyQuizResults(): Observable<QuizResult[]> {
        return this.http.get<QuizResult[]>(`/api/quiz/my`);
    }

    getQuizResultByPage(pageId: string): Observable<QuizResult[]> {
        return this.http.get<QuizResult[]>(`/api/quiz?pageId=${pageId}`);
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

export type PageInfo = {
    id: string;
    name: string;
};

export type PageTests = PageInfo & {
    quizzes: QuizResult[];
};
