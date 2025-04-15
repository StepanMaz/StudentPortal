import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { AuthService } from '@services/auth/auth.service';
import { PageComponent } from '../../components/shared/page/page.component';

const Questions: {
    question: string;
    answer: string;
}[] = [
    {
        question: 'Where can i find source code for this project?',
        answer: 'You can find this project code here: https://github.com/StepanMaz/StudentPortal',
    },
    {
        question: "What is this project's stack?",
        answer: 'Angular, ASP.NET, Blazor',
    },
];

@Component({
    selector: 'page-faq',
    standalone: true,
    imports: [MatExpansionModule, CommonModule, PageComponent],
    template: `<app-page>
        <h1 class="text-3xl mb-6">Commonly asked questions</h1>
        <div class="flex flex-col gap-4">
            <mat-expansion-panel *ngFor="let question of questions">
                <mat-expansion-panel-header>{{ question.question }}</mat-expansion-panel-header>
                <p>{{ question.answer }}</p>
            </mat-expansion-panel>
        </div>
    </app-page>`,
    styles: ``,
})
export class FAQPageComponent {
    questions = Questions;
}
