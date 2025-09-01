import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from './shared/components/header/header';
import { Footer } from './shared/components/footer/footer'; 
import { ListTrucks } from './modules/trucks/views/pages/list-trucks/list-trucks';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Header, Footer, ListTrucks],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('truck-control');
}
