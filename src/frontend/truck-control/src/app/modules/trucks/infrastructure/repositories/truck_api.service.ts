// src/app/modules/produtos/infrastructure/repositories/produto-api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TruckDTO, CreateTruckDTO } from '../../domain/entities/truck.model';
import { TruckRepository } from '../../domain/repositories/truck.repository.interface';
import { ApiService } from '../../../../core/services/api/api.service';

@Injectable({ providedIn: 'root' })
export class TruckApiService implements TruckRepository {
  private readonly API_URL = 'trucks';

  constructor(
    private http: HttpClient,
    private api: ApiService
  ) {}

  listTrucks(): Observable<TruckDTO[]> {
    console.log(localStorage.getItem('authToken'))
    return this.api.get<TruckDTO[]>(this.API_URL);
  }

  getById(id: number): Observable<TruckDTO> {
    return this.api.get<TruckDTO>(`${this.API_URL}/${id}`);
  }

  create(truck: CreateTruckDTO): Observable<TruckDTO> {
    return this.api.post<TruckDTO>(this.API_URL, truck);
  }

  update(truck: TruckDTO): Observable<TruckDTO> {
    return this.api.put<TruckDTO>(`${this.API_URL}/${truck.id}`, truck);
  }

  delete(id: number ): Observable<void> {
    return this.api.delete<void>(`${this.API_URL}/${id}`);
  }
}
