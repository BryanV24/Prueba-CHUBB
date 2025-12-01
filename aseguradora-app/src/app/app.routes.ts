import { Routes } from '@angular/router';
import { AseguradosComponent } from './Componentes/Asegurados/componentesAseguradosModal/asegurados.component';
import { SegurosComponent } from './Componentes/Seguros/componenteSeguroModal/seguros.component';
import { BandejaAseguradosComponent } from './Componentes/Asegurados/bandejaAsegurados/bandejaAsegurados.component';
import { BandejaSegurosComponent } from './Componentes/Seguros/bandejaSeguros/bandejaSeguros.component';
import { NavegadorMenuComponent } from './Shared/navegadorMenu.component';
import { LoginComponent } from './Componentes/login/login.component';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'navegadorMenu', component: NavegadorMenuComponent,
    children: [
      { path: 'bandejaAsegurados', component: BandejaAseguradosComponent },
      { path: 'asegurados', component: AseguradosComponent },
      { path: 'bandejaSeguros', component: BandejaSegurosComponent },
      { path: 'seguros', component: SegurosComponent },
    ]
  },
];  
