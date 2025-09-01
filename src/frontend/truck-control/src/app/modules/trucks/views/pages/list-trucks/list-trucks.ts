import { AfterViewInit, Component } from '@angular/core';
import { Router } from '@angular/router';
import { TruckService } from '../../../application/services/truck.service';
import { CreateTruckDTO, TruckDTO } from '../../../domain/entities/truck.model';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-list-trucks',
  standalone: true,
  imports: [ CommonModule, MatButtonModule, MatDividerModule, MatIconModule, MatCardModule],
  templateUrl: './list-trucks.html',
  styleUrl: './list-trucks.scss'
})
export class ListTrucks{
  trucks: any[] = [];
  loading = true;

  constructor(
    private truckService: TruckService,
    private router: Router
  ) {}

  ngOnInit(){
    this.getTrucks().subscribe((response) => {
      this.trucks = response
      this.loading = false
    });
  }

  getTrucks(){
    return this.truckService.getAll();
  }

  onClickDelete(id: number){
    this.deleteTruck(id)
  }

  onEditTruck(id: number) {
    this.router.navigate(['/update', id])
  }

  deleteTruck(id: number){
   this.truckService.delete(id).subscribe({
    next: () => {
        console.log('Deletado com sucesso');
        this.trucks = this.trucks.filter(truck => truck.id !== id);
    },
    error: (error) => {
        console.error('Erro ao deletar:', error);
    }
});
  }

  redirectToNew(){
    this.router.navigate(['/new'])
  }
}
