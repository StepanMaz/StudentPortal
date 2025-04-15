import { Component } from '@angular/core';
import { InfoCardComponent } from '../../components/landing/info-card/info-card.component';

@Component({
    selector: 'page-root',
    standalone: true,
    imports: [InfoCardComponent],
    template: `
        <div class="page p-8 pt-15">
            <h1 class="text-4xl font-bold mb-5">Empowering Teachers, Enhancing Learning</h1>
            <p class="text-2xl mb-24">
                StudentPortal is an innovative platform designed for educators to create, share, and edit educational
                materials and tests effortlessly. Save time, stay organized, and improve student assessment—all in one
                place.
            </p>
            <div class="flex flex-col gap-5 items-center relative">
                <img class="absolute ellipses invisible xl:visible" src="images/ellipses.svg" />
                <h2 class="text-6xl font-medium">How it works</h2>
                <div class="flex flex-col lg:flex-row gap-16">
                    <landing-info-card
                        border_color="#A9B5DF"
                        icon="timer"
                        text="Quickly create, edit, and reuse materials to reduce prep time and focus on teaching."
                    />
                    <landing-info-card
                        border_color="#7886C7"
                        icon="inventory"
                        text="Keep lessons, tests, and resources neatly stored and easily accessible in one place."
                    />
                    <landing-info-card
                        border_color="#A9B5DF"
                        icon="star"
                        text="Automate grading, provide instant feedback, and track student progress effortlessly."
                    />
                </div>
            </div>
        </div>
    `,
    styles: `
        .ellipses {
            transform: translate(-50%, -50%);
            left: -60px;
            z-index: -1;
        }
    `,
})
export class RootPageComponent {}
