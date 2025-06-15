import { Component } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-view-page',
    standalone: true,
    imports: [],
    template: `@if (iframeUrl; as url) {
        <iframe [src]="url" width="100%" height="100%" style="border:none;"></iframe>
    }`,
    styles: ``,
})
export class ViewPageComponent {
    iframeUrl!: SafeResourceUrl;

    constructor(
        private route: ActivatedRoute,
        private sanitizer: DomSanitizer,
    ) {
        this.route.url.subscribe((segments) => {
            const path = segments.map((seg) => seg.path).join('/');
            this.iframeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
                `${new URL(window.location.href).origin}/pages/view/${path}`,
            );
        });
    }
}
