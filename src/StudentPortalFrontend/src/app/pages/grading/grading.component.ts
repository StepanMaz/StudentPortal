import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router } from '@angular/router';
import { QuizResult, QuizService } from '@services/quiz/quiz.service';
import { stringify } from 'querystring';
import { BehaviorSubject, Observable, of, shareReplay } from 'rxjs';
import { UserBadgeComponent } from '../../components/shared/user-badge/user-badge.component';
import { User } from '@lib/user';
import { UserService } from '@services/user/user-service.service';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'page-grading',
    standalone: true,
    imports: [
        CommonModule,
        MatCardModule,
        MatInputModule,
        MatFormFieldModule,
        UserBadgeComponent,
        MatButtonModule,
        FormsModule,
    ],
    template: `<div class="page">
        <h1>Test: {{ pageName | async }}</h1>
        @if (user | async; as u) {
            <div class="my-4 flex justify-center">
                <mat-card class="w-fit flex gap-2 flex-row items-center p-2" [style.flex-direction]="'row'">
                    Grading for: <app-user-badge [user]="u" />
                </mat-card>
            </div>
        }
        @if (quiz$ | async; as quiz) {
            <div class="flex flex-col gap-2">
                @for (item of quiz.data; track item) {
                    <mat-card>
                        <mat-card-header>
                            <div class="flex justify-between w-full">
                                <span> Question: {{ item.question.text }} </span>
                                <div class="flex gap-1 items-center">
                                    Score:
                                    <div>
                                        <input
                                            matInput
                                            type="number"
                                            class="bg-transparent placeholder:text-slate-400 text-slate-700 text-sm border border-slate-200 rounded-md px-3 py-2 transition duration-300 ease focus:outline-none focus:border-slate-400 hover:border-slate-300 shadow-sm focus:shadow w-18 no-spinner"
                                            [(ngModel)]="item.score.score"
                                            [max]="item.score.maxScore"
                                            [min]="0"
                                        />
                                    </div>
                                    / {{ item.score.maxScore }}
                                </div>
                            </div>
                        </mat-card-header>
                        <mat-card-content> Answer: {{ item.answer.text }} </mat-card-content>
                    </mat-card>
                }
                <button
                    mat-raised-button
                    color="primary"
                    class="my-2 m-auto"
                    [style.display]="'block'"
                    (click)="Save(quiz)"
                >
                    Save
                </button>
            </div>
        }
    </div>`,
    styles: `
        .no-spinner::-webkit-outer-spin-button,
        .no-spinner::-webkit-inner-spin-button {
            -webkit-appearance: none;
            margin: 0;
        }

        .no-spinner[type='number'] {
            -moz-appearance: textfield;
        }
    `,
})
export class GradingComponent implements OnInit {
    testId!: string;
    quiz$!: Observable<QuizResult>;
    pageName = new BehaviorSubject('');
    user = new BehaviorSubject<User | undefined>(undefined);

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private quizService: QuizService,
        private userService: UserService,
        private httpClient: HttpClient,
    ) {}

    ngOnInit(): void {
        this.testId = this.route.snapshot.paramMap.get('id')!;
        this.quiz$ = this.quizService.getQuizResult(this.testId).pipe(shareReplay(1));
        this.quiz$.subscribe((x) =>
            this.httpClient
                .get<{ id: string; name: string }>(`/api/pages/info/${x.quizId}`)
                .subscribe((x) => this.pageName.next(x.name)),
        );
        this.quiz$.subscribe((x) => this.userService.getUser(x.userId).subscribe((x) => this.user.next(x)));
    }

    Save(quiz: QuizResult) {
        this.quizService.publish(quiz).subscribe((x) => this.router.navigate(['tests']));
    }
}
