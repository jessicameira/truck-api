// src/app/modules/produtos/domain/repositories/produto.repository.interface.ts
import { Observable } from 'rxjs';
import { TruckDTO, CreateTruckDTO } from '../entities/truck.model';
import { ResponseGeneric } from '../../../../core/interfaces/response.api';

export abstract class TruckRepository {
  abstract listTrucks(): Observable<TruckDTO[]>;
  abstract getById(id: number): Observable<TruckDTO>;
  abstract create(truck: CreateTruckDTO ): Observable<TruckDTO>;
  abstract update(truck: TruckDTO): Observable<TruckDTO>;
  abstract delete(id: number): Observable<void>;
}