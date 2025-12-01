import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-root ',
  imports: [ RouterModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App { }