import { ChangeDetectorRef, Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { AseguradosService } from '../../../Cores/Servicios/asegurados.services';
import { FormsModule } from '@angular/forms';
import { CampoConfig } from '../../../Cores/Models/camposReusablesConfig';
import { Asegurado } from '../../../Cores/Models/asegurados.model';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { AsignacionService } from '../../../Cores/Servicios/asignarSeguroAsegurado.service';
import { AsignacionSeguro } from '../../../Cores/Models/asignarSegurosAsegurados.model';
import { AuthService } from '../../../Services/auth';

@Component({
  selector: 'app-asegurados',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './asegurados.component.html',
  styleUrls: ['./asegurados.component.css']
})
export class AseguradosComponent implements OnChanges {
    
  constructor(
    private aseguradosServicio: AseguradosService,
    private asignarServicio: AsignacionService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}
  @Input() formModel: any = {seguros: []};
  @Output() cerrarModalEvent = new EventEmitter<void>();
  @Input() actualizar: boolean = false;
  @Input() asignar: boolean = false;
  @Input() consultar: boolean = false;
  mostrarModal = false;
  datos: Asegurado[] = [];
  asignacion: any[] = [];
  seguros: any[] = [];
  usuario: any;
  segurosConsultados: any[] = [];
  campos: CampoConfig[] = [
    { key: 'aseguradoId', type:'number', hidden: true},
    { key: 'nombreCompleto', label:'Nombre Completo', type:'text', required:true },
    { key: 'cedula', label:'Cédula', type:'text', required:true, minLength:10, maxLength:10 },
    { key: 'telefono', label:'Teléfono', type:'text', required:true, minLength:10, maxLength:10 },
    { key: 'edad', label:'Edad', type:'text', required:true, minLength:1, maxLength:120 }
  ];


  ngOnChanges(changes: SimpleChanges) {
    if (changes['formModel'] && this.formModel) {
      this.seguros = this.formModel.seguros || [];
      if (this.consultar) {
        this.asignacion = this.seguros.map((s:any) => s.seguroId);
      }
      this.cdr.detectChanges();
    }
  }

  ngOnInit() {
    this.usuario = this.authService.obtenerUsuario();
    if (this.consultar) {
      this.segurosConsultados = this.formModel.seguros ?? [];
    }
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

  //METODO PARA GUARDAR O ACTUALIZAR UN ASEGURADO
  guardar() {     
    const usuario = "Bryan Vera" ; 
    if (this.actualizar) {
      const data = { ...this.formModel };
      this.aseguradosServicio.actualizarAsegurado(data, usuario).subscribe({
        next: () => {
          Swal.fire({
            title: 'Actualizado',
            text: 'El asegurado fue actualizado correctamente',
            icon: 'success',
            showConfirmButton: true,
            allowOutsideClick: false,
            allowEscapeKey: false
          });
          this.cerrarModal();
          this.cargarAsegurados();
        },
        error: err => {
          Swal.fire({
            title: 'Error',
            text: 'No se pudo actualizar el asegurado',
            icon: 'error'
          });
          console.error('Error al actualizar asegurado', err);
        }
      }); 
    }
    else{
      const data = { ...this.formModel };
      data.edad = Number(data.edad);
      //validaciones para los campos del formulario
      if (data.edad === 0) {
        Swal.fire({
          title: 'Advertencia',
          text: 'La edad no puede ser cero',
          icon: 'warning'
        });
        return;
      }
      this.aseguradosServicio.insertarAsegurado(data, usuario).subscribe({
        next: () => {
          Swal.fire({
            title: 'Creado',
            text: 'El asegurado fue creado correctamente',
            icon: 'success',
            showConfirmButton: true,
            allowOutsideClick: false,
            allowEscapeKey: false
          });
          this.cerrarModal();
          this.cargarAsegurados();
        },
        error: err => {
          Swal.fire({
            title: 'Error',
            text: err .error.message || 'No se pudo crear el asegurado',
            icon: 'error'
          });
          console.error('Error al crear asegurado', err);
        }   
      });
    }
  } 
  
  toggleSeguro(seguroId: number, checked: boolean) {
    if (!this.asignacion) {
      this.asignacion = [];
    }
    if (checked) {
      // agregar solo si no existe y asegurarse que es número
      if (!this.asignacion.includes(seguroId)) {
        this.asignacion.push(Number(seguroId));
    }
  } else {
    // eliminar solo por ID
    this.asignacion = this.asignacion.filter(
      (id: number) => id !== seguroId
    );
  }
}
  //METODO PARA ASIGNAR SEGUROS AL ASEGURADO
  asignarSeguros() {
    let asignacion: AsignacionSeguro = {
      aseguradoId: this.formModel.aseguradoId,
      seguroId: this.asignacion
    };
    this.asignarServicio.asignarMultipleSeguros(asignacion).subscribe({
      next: () => {
        Swal.fire({
          title: 'Asignado',
          text: 'Los seguros fueron asignados correctamente',
          icon: 'success',  
          showConfirmButton: true,
          allowOutsideClick: false,
          allowEscapeKey: false
        });
        this.cerrarModal();
        this.cargarAsegurados();
      },
      error: err => {
        Swal.fire({
          title: 'Error',
          text: 'No se pudieron asignar los seguros',
          icon: 'error'
        });
        console.error('Error al asignar seguros', err);
      }
    });
  }



  //METODO PARA CERRAR EL MODAL
  cerrarModal() {
    this.mostrarModal = false;
    console.log(this.formModel);
    this.cerrarModalEvent.emit();
  }
}
