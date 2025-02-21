import { Component } from '@angular/core';

@Component({
  selector: 'app-logo',
  standalone: true,
  imports: [],
  template: `
    <div class="container">
      <span class="higher">Student</span>
      <span class="lower"> Portal</span>
    </div>
  `,
  styles: `
    @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@500;900&display=swap');

    .container  {
      background-color:#1e5068;
      color: white;
      padding: 0.5em 1em;
      width: max-content;
      border-radius: 2em;

      font-family: "Poppins", sans-serif;
      font-weight: 800;
      font-size: 24px;
    }
    
    .higher {
      position: relative;
      top: -5px;
    }

    .lower {
      position: relative;
      bottom: -5px;
    }
    `,
})
export class LogoComponent {}
