import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDividerModule } from '@angular/material/divider';
import { TruckService } from '../../../application/services/truck.service';
import { CreateTruckDTO, TruckDTO } from '../../../domain/entities/truck.model';
import { Router, ActivatedRoute } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomSelectComponent } from '../../../../../shared/components/custom-select/custom-select';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-new',
  imports: [CommonModule, MatDividerModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, MatIconModule, ReactiveFormsModule, CustomSelectComponent],
  templateUrl: './new.html',
  styleUrl: './new.scss'
})
export class New {
  truck!: TruckDTO;
  loading = true;
  id!: number
  truckForm: FormGroup;

  modelOptions = [
    { value: 1, label: 'FH' },
    { value: 2, label: 'FM' },
    { value: 3, label: 'VM' },
  ];

  plantOptions = [
    { value: 1, label: 'BR' },
    { value: 2, label: 'FR' },
    { value: 3, label: 'SE' },
    { value: 4, label: 'US' },

  ];

  constructor(
    private truckService: TruckService,
    private activatedRoute: ActivatedRoute,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private router: Router
  ) {
     this.truckForm = this.fb.group({
      model: ['', Validators.required],
      chassisNumber: ['', Validators.required],
      color: ['', Validators.required],
      yearManufacture: [0, [Validators.required, Validators.pattern(/^\d{4}$/)]],
      plant: [0, [Validators.required]],
    });
  }

  ngOnInit(){
    this.loading = false
  }

  createTruck(){
    const truck: CreateTruckDTO = {
      model: this.truckForm.get('model')?.value,
      chassisNumber: this.truckForm.get('chassisNumber')?.value,
      color: this.truckForm.get('color')?.value,
      yearManufacture: this.truckForm.get('yearManufacture')?.value,
      plant: this.truckForm.get('plant')?.value
    }

    this.truckService.create(truck).subscribe(reponse => {
      console.log("Registro criado com sucesso")
      this.router.navigate(['/'])
    }, (error: HttpErrorResponse) => {
      console.log("erro ao atualizar registro ")
    })
  }

  onSubmit(){
    if(this.truckForm.valid) {
      this.createTruck();
    } else {
      console.log("preencha todos os campos.")
    }
  }

  resetField(){
    console.log('reset')
  }

  onCancel(){
    this.router.navigate(['/'])
  }
}
