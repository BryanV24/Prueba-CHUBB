import { ChangeDetectorRef, Component} from '@angular/core';
import { CampoConfig } from '../../../Cores/Models/camposReusablesConfig';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { VariablesReusablesSegurosComponent } from '../componenteReusableSeguros/variablesReusablesSeguros.component';
import { Seguro } from '../../../Cores/Models/seguros.model';
import { SegurosService } from '../../../Cores/Servicios/seguros.service';
import { SegurosComponent } from '../componenteSeguroModal/seguros.component';

@Component({
  selector: 'app-bandeja-seguros',
  standalone: true,
  imports: [VariablesReusablesSegurosComponent, CommonModule, SegurosComponent],
  templateUrl: './bandejaSeguros.component.html',
  styleUrls: ['./bandejaSeguros.component.css']
})
export class BandejaSegurosComponent {
    
  constructor(
    private SeguroServicio: SegurosService,
    private cdr: ChangeDetectorRef
  ) {}
  
  mostrarModal = false;
  formModel: Seguro | null = null;
  actualizar = false;
  campos: CampoConfig[] = [
    { key: 'seguroId', label:'Secuencia', type:'number'},
    { key: 'nombreSeguro', label:'Nombre Seguro', type:'text', required: true },
    { key: 'codigoSeguro', label:'Código Seguro', type:'text', required: true, minLength:10, maxLength:10 },
    { key: 'sumaAsegurada', label:'Suma Asegurada', type:'number', required: true, minLength:10, maxLength:10 },
    { key: 'prima', label:'Prima', type:'number', required: true, minLength:1, maxLength:120 },
    { key: 'accion', label:'Acciones', type:'actions'},
  ];
  datos: Seguro[] = [];

  ngOnInit() {
    this.cargarSeguros();
  }

  //METODO PARA PODER CONSULTAR TODOS LOS SEGUROS
  cargarSeguros() {
    this.SeguroServicio.obtenerSeguros().subscribe({
        next: (resp: Seguro[]) => {
          this.datos = resp;
          this.cdr.detectChanges();
        },
        error: err => console.error('Error al cargar asegurados', err)
    });
  }

  //METODO PARA PODER ACTUALIZAR LOS SEGUROS
  editarSeguro(seguro: Seguro) {
    // Pre-cargar datos en el modal
    this.formModel = { ...seguro };
    this.actualizar = true;
    this.mostrarModal = true;
    this.cdr.detectChanges();
  }

  //METODO PARA PODER ELIMINAR LOS SEGUROS
  eliminarSeguro(seguro: Seguro) {
    Swal.fire({
      title: 'Advertencia',
      text: `¿Esta seguro de eliminar el seguro ${seguro.nombreSeguro}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6'
    }).then((result) => {
      if (result.isConfirmed) {
        this.SeguroServicio.eliminarSeguro(seguro.seguroId).subscribe({
            next: () => {
              Swal.fire({
                title: 'Eliminado',
                text: 'El seguro fue eliminado correctamente',
                icon: 'success',
                timer: 2000,
                showConfirmButton: false
              });
              //una vez eliminado recargar la pagina con la lista actualizada
              this.cargarSeguros();
            },
            error: () => {
              Swal.fire({
                title: 'Error',
                text: 'No se pudo eliminar el registro',
                icon: 'error'
              });
            }
        });
      }
    });
  }

  //METODO PARA CERRAR EL MODAL
  cerrarModal() {
    this.mostrarModal = false;
    this.formModel = null;
    this.cargarSeguros();
  }
}