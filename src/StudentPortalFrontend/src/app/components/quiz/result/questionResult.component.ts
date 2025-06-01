import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

@Component({
    selector: 'quiz-question-score',
    standalone: true,
    imports: [MatCardModule, CommonModule],
    template: `
        <mat-card [ngClass]="scoreClass">
            <mat-card-title>{{ question }}</mat-card-title>
            <mat-card-content>
                <p><strong>Answer:</strong> {{ answer }}</p>
                <p><strong>Score:</strong> {{ scoreValue }} / {{ scoreMax }}</p>
            </mat-card-content>
        </mat-card>
    `,
    styles: [
        `
            mat-card {
                margin: 10px;
                padding: 16px;
                border-radius: 12px;
                color: #fff;
            }
            .score-red {
                background-color: #f44336;
            }
            .score-yellow {
                background-color: #ffeb3b;
                color: #000;
            }
            .score-green {
                background-color: #4caf50;
            }
        `,
    ],
})
export class QuestionScoreComponent {
    @Input() question!: string;
    @Input() answer!: string;
    @Input() scoreValue!: number;
    @Input() scoreMax!: number;

    get scoreClass(): string {
        if (this.scoreValue === 0) return 'score-red';
        if (this.scoreValue < this.scoreMax) return 'score-yellow';
        return 'score-green';
    }
}
