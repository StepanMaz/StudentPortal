import { ActivatedRoute } from '@angular/router';
import { QuizResult, QuizService } from '@services/quiz/quiz.service';
import { Observable } from 'rxjs';
import { WithLoadingPipe } from '@pipes/with-loading.pipe';
import { QuestionScoreComponent } from '../../components/quiz/result/questionResult.component';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-results',
    standalone: true,
    imports: [CommonModule, WithLoadingPipe, QuestionScoreComponent],
    template: `<div class="page" *ngIf="quizResult$ | withLoading | async as quizRes">
        <div *ngIf="quizRes.loading; else loaded"></div>
        <ng-template #loaded>
            <ng-template [ngIf]="quizRes.value">
                <span>Your Score is {{ getScore(quizRes.value) }} / {{ getMaxScore(quizRes.value) }}</span>
                <quiz-question-score
                    *ngFor="let res of quizRes.value.data"
                    [answer]="res.answer.text"
                    [question]="res.question.text"
                    [scoreValue]="res.score.score"
                    [scoreMax]="res.score.maxScore"
                />
            </ng-template>
            <ng-template [ngIf]="quizRes.error">Not Found</ng-template>
        </ng-template>
    </div>`,
    styles: ``,
})
export class ResultsPage {
    resultId!: string;
    quizResult$!: Observable<QuizResult>;

    constructor(
        private route: ActivatedRoute,
        private quizService: QuizService,
    ) {}

    getScore(quizRes: QuizResult) {
        return quizRes.data.reduce((sum, { score: { score } }) => sum + score, 0);
    }

    getMaxScore(quizRes: QuizResult) {
        return quizRes.data.reduce((sum, { score: { maxScore } }) => sum + maxScore, 0);
    }

    ngOnInit(): void {
        this.resultId = this.route.snapshot.paramMap.get('id')!;
        this.quizResult$ = this.quizService.getQuizResult(this.resultId);
    }
}

type LoadingState = { loading: true } | { loading: false; error: string } | { loading: false; value: QuizResult };
