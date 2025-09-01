import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDividerModule } from '@angular/material/divider';
import { TruckService } from '../../../application/services/truck.service';
import { TruckDTO } from '../../../domain/entities/truck.model';
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
  selector: 'app-edit',
  imports: [CommonModule, MatDividerModule, ReactiveFormsModule, MatFormFieldModule, MatSelectModule, MatInputModule, MatButtonModule, MatIconModule, CustomSelectComponent],
  templateUrl: './edit.html',
  styleUrl: './edit.scss'
})
export class Edit {
  truck!: TruckDTO;
  loading = true;
  id!: number
  truckForm: FormGroup;

  // Opções para os selects
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
      yearManufacture: [0, [Validators.required]],
      plant: [0, [Validators.required]],
    });
  }

  ngOnInit(){
    this.id = this.activatedRoute.snapshot.params['id']
    this.getTruck(this.id).subscribe((response) => {
      this.loading = false
      this.truckForm.patchValue(response);
      console.log(this.truck)
      console.log(response)
    });
  }

  getTruck(id: number){
    return this.truckService.getById(id);
  }

  updateTruck(): void{
    const truck: TruckDTO = {
      id: this.id,
      model: this.truckForm.get('model')?.value,
      chassisNumber: this.truckForm.get('chassisNumber')?.value,
      color: this.truckForm.get('color')?.value,
      yearManufacture: this.truckForm.get('yearManufacture')?.value,
      plant: this.truckForm.get('plant')?.value
    }

    this.truckService.update(truck).subscribe(reponse => {
      console.log("Registro atualizado com sucesso")
      this.router.navigate(['/'])
    }, (error: HttpErrorResponse) => {
      console.log("erro ao atualizar registro ")
    })
  }

  onSubmit(){
    if(this.truckForm.valid) {
      this.updateTruck();
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
