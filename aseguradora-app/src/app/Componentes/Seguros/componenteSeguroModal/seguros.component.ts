import { CommonModule } from "@angular/common";
import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { SegurosService } from "../../../Cores/Servicios/seguros.service";
import { Seguro } from "../../../Cores/Models/seguros.model";
import { CampoConfig } from "../../../Cores/Models/camposReusablesConfig";
import Swal from "sweetalert2";

@Component({
  selector: 'app-seguros',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: 'seguros.component.html',
  styleUrls: ['seguros.component.css']
})
export class SegurosComponent {

  constructor(
    private segurosServicio: SegurosService,
    private cdr: ChangeDetectorRef
  ) {}
  @Input() formModel: any = {};
  @Output() cerrarModalEvent = new EventEmitter<void>();
  @Input() actualizar: boolean = false;
  mostrarModal = false;
  datos: Seguro[] = [];
  campos: CampoConfig[] = [
    { key: 'seguroId', type:'number', hidden: true},
    { key: 'nombreSeguro', label:'Nombre Seguro', type:'text', required: true },
    { key: 'codigoSeguro', label:'Código Seguro', type:'text', required: true, minLength:10, maxLength:10 },
    { key: 'sumaAsegurada', label:'Suma Asegurada', type:'text', required: true, minLength:10, maxLength:10 },
    { key: 'prima', label:'Prima', type:'text', required: true, minLength:1, maxLength:120 }
  ];


  //METODO PARA PODER CONSULTAR TODOS LOS SEGUROS
  cargarSeguros() {
    this.segurosServicio.obtenerSeguros().subscribe({
        next: (resp: Seguro[]) => {
          this.datos = resp;
          this.cdr.detectChanges();
          console.log('Datos cargados:', this.datos);
        },
        error: err => console.error('Error al cargar seguros', err)
    });
  }

  //METODO PARA GUARDAR O ACTUALIZAR UN SEGURO
  guardar() {     
    const usuario = "Bryan Vera" ;
    if (this.actualizar) {
      const data = { ...this.formModel };
      //en el form viene como texto y aqui se lo convierte en numeros para que puedan ser registrados en la base de datos
      data.sumaAsegurada = Number(data.sumaAsegurada);
      data.prima = Number(data.prima);
      this.segurosServicio.actualizarSeguro(data, usuario).subscribe({
        next: () => {
          Swal.fire({
            title: 'Actualizado',
            text: 'El seguro fue actualizado correctamente',
            icon: 'success',
            showConfirmButton: true,
            allowOutsideClick: false,
            allowEscapeKey: false
          });
          this.cerrarModal();
          this.cargarSeguros();
        },
        error: err => {
          Swal.fire({
            title: 'Error',
            text: 'No se pudo actualizar el seguro',
            icon: 'error'
          });
          console.error('Error al actualizar seguro', err);
        }
      }); 
    }
    else{
      const data = { ...this.formModel };
      //en el form viene como texto y aqui se lo convierte en numeros para que puedan ser registrados en la base de datos
      data.sumaAsegurada = Number(data.sumaAsegurada);
      data.prima = Number(data.prima);
      this.segurosServicio.insertarSeguro(data, usuario).subscribe({
        next: () => {
          Swal.fire({
            title: 'Creado',
            text: 'El seguro fue creado correctamente',
            icon: 'success',
            showConfirmButton: true,
            allowOutsideClick: false,
            allowEscapeKey: false
          });
          this.cerrarModal();
          this.cargarSeguros();
        },
        error: err => {
          Swal.fire({
            title: 'Error',
            text: 'No se pudo crear el seguro',
            icon: 'error'
          });
          console.error('Error al crear seguro', err);
        }   
      });
    }
  }

  //METODO PARA CERRAR EL MODAL
  cerrarModal() {
    this.mostrarModal = false;
    this.cerrarModalEvent.emit();
  }
}