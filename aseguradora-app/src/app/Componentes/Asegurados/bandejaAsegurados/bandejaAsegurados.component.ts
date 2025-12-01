import { ChangeDetectorRef, Component, Input, ViewChild } from '@angular/core';
import { AseguradosService } from '../../../Cores/Servicios/asegurados.services';
import { CampoConfig } from '../../../Cores/Models/camposReusablesConfig';
import { VariablesReusablesComponent } from '../componenteReusableAsegurados/variablesReusables.component';
import { Asegurado } from '../../../Cores/Models/asegurados.model';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { AseguradosComponent } from '../componentesAseguradosModal/asegurados.component';

@Component({
  selector: 'app-bandeja-asegurados',
  standalone: true,
  imports: [VariablesReusablesComponent, CommonModule, AseguradosComponent],
  templateUrl: './bandejaAsegurados.component.html',
  styleUrls: ['./bandejaAsegurados.component.css']
})
export class BandejaAseguradosComponent {
    
  constructor(
    private aseguradosServicio: AseguradosService,
    private cdr: ChangeDetectorRef
  ) {}
  
  //DECLARACIONES DE LAS VARIABLES
  @ViewChild(AseguradosComponent) aseguradosComponente!: AseguradosComponent;

  mostrarModal = false;
  formModel: Asegurado | null = null;
  actualizar = false;
  asignar = false;
  consultar = false;
  campos: CampoConfig[] = [
    { key: 'aseguradoId', label:'Secuencia', type:'number'},
    { key: 'nombreCompleto', label:'Nombre Completo', type:'text', required: true },
    { key: 'cedula', label:'Cédula', type:'text', required: true, minLength:10, maxLength:10 },
    { key: 'telefono', label:'Teléfono', type:'number', required: true, minLength:10, maxLength:10 },
    { key: 'edad', label:'Edad', type:'number', required: true, minLength:1, maxLength:120 },
    { key: 'accion', label:'Acciones', type:'actions'},
  ];
  datos: Asegurado[] = [];

  ngOnInit() {
    this.cargarAsegurados();
  }

  //METODO PARA PODER CONSULTAR TODOS LOS ASEGURADOS
  cargarAsegurados() {
    this.aseguradosServicio.obtenerAsegurados().subscribe({
        next: (resp: Asegurado[]) => {
          this.datos = resp;
          this.cdr.detectChanges();
        },
        error: err => console.error('Error al cargar asegurados', err)
    });
  }

  //METODO PARA PODER ACTUALIZAR LOS ASEGURADOS 
  editarAsegurado(registro: Asegurado) {
    // Pre-cargar datos en el modal
    this.formModel = { ...registro };
    this.actualizar = true;
    this.asignar = false;
    this.mostrarModal = true;
    this.consultar = false;
    this.cdr.detectChanges();
  }

  //METODO PARA PODER ELIMINAR LOS ASEGURADOS
  eliminarAsegurado(asegurado: Asegurado) {
    Swal.fire({
      title: 'Advertencia',
      text: `¿Está seguro de eliminar al asegurado ${asegurado.nombreCompleto}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar',
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6'
    }).then((result) => {
      if (result.isConfirmed) {
        this.aseguradosServicio.eliminarAsegurado(asegurado.aseguradoId).subscribe({
            next: () => {
              Swal.fire({
                title: 'Eliminado',
                text: 'El asegurado fue eliminado correctamente',
                icon: 'success',
                timer: 2000,
                showConfirmButton: false
              });
              //una vez eliminado recargar la pagina con la lista actualizada
              this.cargarAsegurados();
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

  //METODO PARA ASIGNAR SEGUROS A UN ASEGURADO
  asignarSeguros(asegurado: any, seguros: any) {
    // Pre-cargar datos en el modal
    this.formModel = { ...asegurado, seguros};
    this.actualizar = false;
    this.asignar = true;
    this.mostrarModal = true;
    this.consultar = false;
    this.cdr.detectChanges();
  }

  //METODO PARA CONSULTAR LOS SEGUROS DE UN ASEGURADO
  consultarAseguradosConSeguros(aseguradoSeguros: any) {
    // Pre-cargar datos en el modal
    const asegurado = aseguradoSeguros[0];
    this.formModel = {
      aseguradoId: asegurado.aseguradoId,
      nombreCompleto: asegurado.nombreCompleto,
      cedula: asegurado.cedula,
      seguros: aseguradoSeguros 
    };
    this.actualizar = false;
    this.asignar = false;
    this.mostrarModal = true;
    this.consultar = true;
    this.cdr.detectChanges();
  }

  //METODO PARA CERRAR EL MODAL
  cerrarModal() {
    this.mostrarModal = false;
    this.formModel = null;
    this.cargarAsegurados();
  }
}