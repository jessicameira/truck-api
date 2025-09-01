// src/app/modules/produtos/application/services/produto.service.ts
import { Injectable, inject } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { TruckDTO, CreateTruckDTO } from '../../domain/entities/truck.model';
import { TruckRepository } from '../../domain/repositories/truck.repository.interface';

@Injectable({ providedIn: 'root' })
export class TruckService {
  private truckRepository = inject(TruckRepository);


  getAll(): Observable<TruckDTO[]> {
    return this.truckRepository.listTrucks().pipe(
      catchError((error) => {
        console.error('Erro ao listar itens:', error);
        return throwError(() => new Error('Falha ao carregar itens'));
      })
    );
  }

  getById(id: number): Observable<TruckDTO> {
    if (!id) {
      return throwError(() => new Error('ID obrigatório'));
    }

    return this.truckRepository.getById(id).pipe(
      catchError((error) => {
        console.error(`Erro ao buscar item ${id}:`, error);
        return throwError(() => new Error('Falha ao carregar item'));
      })
    );
  }


  create(produto: CreateTruckDTO): Observable<TruckDTO> {
    return this.truckRepository.create(produto).pipe(
      catchError((error) => {
        console.error('Erro ao criar item:', error);
        return throwError(() => new Error('Falha ao criar item'));
      })
    );
  }

  update(truck: TruckDTO): Observable<TruckDTO> {
    return this.truckRepository.update(truck).pipe(
      catchError((error) => {
        console.error(`Erro ao atualizar item ${truck.id}:`, error);
        return throwError(() => new Error('Falha ao atualizar item'));
      })
    );
  }

  delete(id: number): Observable<void> {
    return this.truckRepository.delete(id).pipe(
      catchError((error) => {
        console.error(`Erro ao excluir item ${id}:`, error);
        return throwError(() => new Error('Falha ao excluir item'));
      })
    );
  }

}