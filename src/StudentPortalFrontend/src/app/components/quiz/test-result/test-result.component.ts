import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { PageTests } from '../../../pages/test-results/test-results.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { QuizResult } from '@services/quiz/quiz.service';
import { SelectionModel } from '@angular/cdk/collections';
import { MatSort } from '@angular/material/sort';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { GradingButtonComponent } from '../grading-button/grading-button.component';
import { Router } from '@angular/router';
import { UserService } from '@services/user/user-service.service';
import { of } from 'rxjs';
import { User } from '@lib/user';
import { UserBadgeComponent } from '../../shared/user-badge/user-badge.component';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-test-result',
    standalone: true,
    imports: [
        MatTableModule,
        MatFormFieldModule,
        MatIconModule,
        MatPaginatorModule,
        MatInputModule,
        GradingButtonComponent,
        UserBadgeComponent,
        CommonModule,
    ],
    template: `
        <div class="page">
            <h1>Test "{{ test.name }}" Grading</h1>

            <table mat-table [dataSource]="test.quizzes">
                <ng-container matColumnDef="User">
                    <th mat-header-cell *matHeaderCellDef>User</th>
                    <td mat-cell *matCellDef="let element" class="pt-2 pb-2">
                        @if (userMap.get(element.userId); as user) {
                            <app-user-badge [user]="user" />
                        } @else {
                            Loading...
                        }
                    </td>
                </ng-container>

                <ng-container matColumnDef="Email">
                    <th mat-header-cell *matHeaderCellDef>Email</th>
                    <td mat-cell *matCellDef="let element">
                        {{ userMap.get(element.userId)?.email }}
                    </td>
                </ng-container>

                <ng-container matColumnDef="Mark">
                    <th mat-header-cell *matHeaderCellDef>Mark</th>
                    <td mat-cell *matCellDef="let element">
                        @if (isGraded(element)) {
                            <app-grading-button
                                Color="#673AB7"
                                (click)="open(element)"
                                InfoText="{{ getScore(element) }}/{{ getMaxScore(element) }}"
                                ButtonText="Details"
                            />
                        } @else {
                            <app-grading-button Color="#ef6c00" (click)="open(element)" ButtonText="Grade" />
                        }
                    </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: displayedColumns"></tr>
            </table>

            <mat-paginator [length]="dataSource.data.length" [pageSize]="15"></mat-paginator>
        </div>
    `,
    styles: ``,
})
export class TestResultComponent implements OnInit {
    displayedColumns = ['User', 'Email', 'Mark'];
    selection!: SelectionModel<any>;
    dataSource!: MatTableDataSource<QuizResult>;

    @Input() test!: PageTests;

    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    constructor(
        public router: Router,
        private userService: UserService,
    ) {}

    userMap = new Map<string, User>();

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource(this.test.quizzes);
        this.selection = new SelectionModel<any>(true, []);
        this.dataSource.sort = this.sort;

        for (const quiz of this.test.quizzes) {
            const sub = this.userService.getUser(quiz.userId).subscribe((u) => {
                this.userMap.set(quiz.userId, u);
                sub.unsubscribe();
            });
        }
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
        this.dataSource.filter = filterValue;
    }

    open(data: QuizResult) {
        this.router.navigate(['tests', 'grading', data.id]);
    }

    isGraded(test: QuizResult) {
        return test.data.every((x) => x.score.score != null);
    }

    getScore(test: QuizResult) {
        return test.data.reduce((sum, x) => x.score.score + sum, 0);
    }

    getMaxScore(test: QuizResult) {
        return test.data.reduce((sum, x) => x.score.maxScore + sum, 0);
    }
}
