import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { RouterModule } from '@angular/router';
import { QuizResult, QuizService } from '@services/quiz/quiz.service';
import { Observable, shareReplay } from 'rxjs';

@Component({
    selector: 'app-my-tests',
    standalone: true,
    imports: [CommonModule, RouterModule, MatCardModule],
    template: `
        <div class="page">
            @if (quizzes$ | async; as quizzes) {
                @if (quizzes.length == 0) {
                    No test results
                } @else {
                    Tests:
                    <mat-card>
                        <mat-card-content>
                            <div class="flex flex-col gap-2">
                                @for (item of quizzes; track item.id) {
                                    <a
                                        routerLink="/results/{{ item.id }}"
                                        class="font-medium text-blue-600 dark:text-blue-500 hover:underline"
                                        >{{ map.get(item.quizId).name }}</a
                                    >
                                }
                            </div>
                        </mat-card-content>
                    </mat-card>
                }
            } @else {
                Loading...
            }
        </div>
    `,
    styles: ``,
})
export class MyTestsComponent implements OnInit {
    quizzes$!: Observable<QuizResult[]>;
    constructor(
        private quizService: QuizService,
        private httpClient: HttpClient,
    ) {}
    map = new Map();
    ngOnInit(): void {
        this.quizzes$ = this.quizService.getMyQuizResults().pipe(shareReplay(1));
        this.quizzes$.subscribe((x) =>
            x.forEach((x) => this.load(x.quizId).subscribe((y) => this.map.set(x.quizId, y))),
        );
    }

    load(quizId: any) {
        return this.httpClient.get<{ name: string }>(`/api/pages/info/${quizId}`);
    }
}
