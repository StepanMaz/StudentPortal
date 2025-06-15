import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDividerModule } from '@angular/material/divider';
import { QuizResult, QuizService } from '@services/quiz/quiz.service';
import { filter, from, map, mergeMap, Observable, toArray } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { TestResultComponent } from "../../components/quiz/test-result/test-result.component";

@Component({
    selector: 'page-test-results',
    standalone: true,
    imports: [CommonModule, MatExpansionModule, MatDividerModule, MatButtonModule, TestResultComponent],
    template: `<div class="page">
        @if (selected) {
            <app-test-result [test]="selected"/>
        } @else {
            @if ($pageTests | async; as pageTests) {
                @for (page of pageTests; track page.id) {
                    <mat-expansion-panel>
                        <mat-expansion-panel-header>
                            <mat-panel-title class="flex gap-2"
                                >Test: {{ page.name }}
                                <button mat-raised-button color="primary" (click)="openTest($event, page)">
                                    Open
                                </button>
                            </mat-panel-title>
                        </mat-expansion-panel-header>
                        {{ page.quizzes.length }} test result{{ page.quizzes.length > 1 ? 's' : '' }}.
                    </mat-expansion-panel>
                }
            }
        }
    </div> `,
    styles: ``,
})
export class TestResultsPageComponent implements OnInit {
    $userPages!: Observable<PageInfo[]>;
    $pageTests!: Observable<PageTests[]>;

    selected: PageTests | undefined;

    constructor(
        private quizService: QuizService,
        private httpClient: HttpClient,
    ) {}

    ngOnInit(): void {
        this.$userPages = this.httpClient.get<PageInfo[]>('/api/pages/list');
        this.$pageTests = this.$userPages.pipe(
            mergeMap((dataArray) => from(dataArray)),
            mergeMap((data) => this.getPageTests(data)),
            filter((result): result is PageTests => !!result),
            toArray(),
        );
    }

    private getPageTests(pageInfo: PageInfo): Observable<PageTests | undefined> {
        return this.quizService
            .getQuizResultByPage(pageInfo.id)
            .pipe(map((x) => (x.length == 0 ? undefined : { ...pageInfo, quizzes: x })));
    }

    public openTest(event: Event, page: PageTests) {
        event.stopPropagation();
        this.selected = page;
    }
}

type PageInfo = {
    id: string;
    name: string;
};

export type PageTests = PageInfo & {
    quizzes: QuizResult[];
};
