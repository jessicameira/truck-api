import { Routes, RouterModule } from '@angular/router';
import { ListTrucks } from './modules/trucks/views/pages/list-trucks/list-trucks';
import { New } from './modules/trucks/views/pages/new/new';
import { Edit } from './modules/trucks/views/pages/edit/edit';

export const routes: Routes = [
    { path: '', component: ListTrucks},
    { path: 'new', component: New},
    { path: 'update/:id', component: Edit}
];
