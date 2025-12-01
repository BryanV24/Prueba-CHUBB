import { ChangeDetectorRef, Component, EventEmitter, Input, Output, ViewChild } from "@angular/core";
import { CampoConfig } from "../../../Cores/Models/camposReusablesConfig";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { AseguradosComponent } from "../componentesAseguradosModal/asegurados.component";
import { AseguradosService } from "../../../Cores/Servicios/asegurados.services";
import { Asegurado } from "../../../Cores/Models/asegurados.model";
import { SegurosService } from "../../../Cores/Servicios/seguros.service";
import * as XLSX from 'xlsx';
import Swal from "sweetalert2";

@Component({
    selector: 'app-variables-reusables',
    standalone: true,
    imports: [FormsModule, CommonModule, AseguradosComponent],
    templateUrl: './variablesReusables.component.html'
})
export class VariablesReusablesComponent {

  constructor(
    private aseguradosServicio: AseguradosService,    
    private seguroServicio: SegurosService,
    private cdr: ChangeDetectorRef
  ) {}
  //DECLARICIONES DE LAS VARIABLES
  @Input() campos: CampoConfig[] = [];
  @Input() datos: any[] = [];
  @Output() onEditar = new EventEmitter<any>();
  @Output() onEliminar = new EventEmitter<any>();
  @Output() onAsignar = new EventEmitter<any>();
  @Output() onConsultar = new EventEmitter<any>();
  @ViewChild(AseguradosComponent) aseguradosComponente!: AseguradosComponent;
  formModel: any = {};
  mostrarModal = false;
  actualizar = false;
  asignar = false;
  consultar = false;    
  archivoSeleccionado: File | null = null;
  listaAsegurados: Asegurado[] = [];
  paginaActual: number = 1;
  registrosPorPagina: number = 5;
  totalPaginas: number = 0;

  ngOnInit() {
    this.cargarAsegurados();
  }

  //METODO PARA PODER CONSULTAR TODOS LOS ASEGURADOS
    cargarAsegurados() {
      this.aseguradosServicio.obtenerAsegurados().subscribe({
          next: (resp: Asegurado[]) => {
            this.datos = resp;
            this.totalPaginas = Math.ceil(this.datos.length / this.registrosPorPagina) || 1;
            this.paginaActual = 1;
            this.cdr.detectChanges();
          },
          error: err => console.error('Error al cargar asegurados', err)
      });
    }

     //METODO PARA CONSULTAR LOS SEGUROS PARA AGINAR A UN ASEGURADO
    consultarSegurosParaAsegurado(asegurado: any) {
      this.seguroServicio.obtenerSeguros().subscribe({
          next: (resp: any) => {
            this.asignarSeguro(asegurado,resp);
          },
          error: err => {
            console.error('Error al obtener seguros para el asegurado', err);
          }
      });
    }

    //METODO PARA CONSULTAR LOS SEGUROS ASIGNADOS A UN ASEGURADO
    ConsultarSegurosDelAsegurado(asegurado: Asegurado) {
      this.aseguradosServicio.obtenerSegurosPorCedula(asegurado.cedula).subscribe({
          next: (resp: any) => {
            this.consultarAseguradosConSeguros(resp)
          },
          error: err => {
            console.error('Error al obtener seguros del asegurado', err);
          }
      });
    }

    //METODO PARA MANEJAR LA SELECCION DE LOS ARCHIVOS
    onFileSelected(event: any) {
      const file = event.target.files[0];
      if (!file) return;
      this.archivoSeleccionado = file;
    }
    
    //METODO PARA PROCESAR LOS ARCHIVOS SELECCIONADOS
    procesarArchivo() {
      if (!this.archivoSeleccionado) {
        Swal.fire({
          title: 'Advertencia',
          text: 'Selecciona un archivo primero',
          icon: 'warning'
        });
        return;
      }

      const reader = new FileReader();

      if (this.archivoSeleccionado.name.endsWith('.txt')) {
        reader.onload = (e: any) => {
          const text = e.target.result;
          this.listaAsegurados = this.procesarTxt(text);
          this.guardarMasivo();
        };
        reader.readAsText(this.archivoSeleccionado);
      } else {
        reader.onload = (e: any) => {
          const data = new Uint8Array(e.target.result);
          const workbook = XLSX.read(data, { type: 'array' });
          const sheetName = workbook.SheetNames[0];
          const sheet = workbook.Sheets[sheetName];
          const jsonData: any[] = XLSX.utils.sheet_to_json(sheet, { defval: '' });

          // Mapear cada fila del Excel a tu modelo
          this.listaAsegurados = jsonData.map(row => ({
            aseguradoId: 0,
            cedula: String(row['cedula'] || row['Cedula'] || '').trim(),
            nombreCompleto: String(row['nombreCompleto'] || row['Nombre'] || '').trim(),
            telefono: String(row['telefono'] || row['Telefono'] || '').trim(),
            edad: Number(row['edad'] || row['Edad'] || 0)
          }));

          this.guardarMasivo();
        };
        reader.readAsArrayBuffer(this.archivoSeleccionado);
      }
    }

    //METODO PARA PROCESAR LOS CAMPOS DENTRO DE LOS ARCHIVOS TXT
    procesarTxt(text: string): Asegurado[] {
      const lineas = text.split('\n').filter(l => l.trim() !== '');
      return lineas.map(linea => {
        const obj: any = {};
        const pares = linea.split(','); // separar cada "clave:valor"
        pares.forEach(par => {
          const [clave, valor] = par.split(':').map(s => s.trim());
          obj[clave] = valor;
        });

        return {
          aseguradoId: 0,
          cedula: obj['cedula'] || '',
          nombreCompleto: obj['nombreCompleto'] || '',
          telefono: obj['telefono'] || '',
          edad: Number(obj['edad'] || 0)
        };
      });
    }


    //METODO PARA GUARDAR LOS ASEGURADOS DE FORMA MASIVA
    guardarMasivo() {
      if (!this.listaAsegurados.length) return;

      const usuario = "Bryan Vera" ;
      this.aseguradosServicio.insertarAseguradosMasivo(this.listaAsegurados, usuario)
        .subscribe({
          next: () => {
            Swal.fire({
              title: 'Creado',
              text: 'Carga de Asegurados de manera masiva fue exitosa',
              icon: 'success',
              showConfirmButton: true,
              allowOutsideClick: false,
              allowEscapeKey: false
            });
            this.cargarAsegurados();
          },
          error: err => console.error('Error al guardar masivo', err)
        });
    }
    //EVENTOS PARA ENVIAR LOS DATOS DE EDITAR Y ELIMINAR
    editar(asegurado: any) {
      this.onEditar.emit(asegurado);
    }    
    eliminar(asegurado: any) {
      this.onEliminar.emit(asegurado);
    }
    asignarSeguro(asegurado: any, seguros: any) {
      this.onAsignar.emit({asegurado, seguros});
    }
    consultarAseguradosConSeguros(aseguradoSeguros: any) {console.log(aseguradoSeguros);
    
      this.onConsultar.emit(aseguradoSeguros);
    }

    //METODO PARA ABRIR EL MODAL DE AGREGAR ASEGURADO
    abrirModal() {
      this.formModel = {};
      this.mostrarModal = true;
      this.actualizar = false;
      this.asignar = false;
      this.cdr.detectChanges();
    }
    //METODO PARA CERRAR EL MODAL
    cerrarModal() {
      this.mostrarModal = false;
      this.formModel = null;
      this.cargarAsegurados();
    }

    //METODO PARA PAGINACION DE LA TABLA
    get datosPagina(): any[] {
      const inicio = (this.paginaActual - 1) * this.registrosPorPagina;
      const fin = inicio + this.registrosPorPagina;
      return this.datos.slice(inicio, fin);
    }
    //METODOS PARA NAVEGAR ENTRE PAGINAS
    siguientePagina() {
      if (this.paginaActual < this.totalPaginas) this.paginaActual++;
      this.cdr.detectChanges();
    }
    paginaAnterior() {
      if (this.paginaActual > 1) this.paginaActual--;
      this.cdr.detectChanges();
    }
    irAPagina(p: number) {
      if (p >= 1 && p <= this.totalPaginas) this.paginaActual = p;
      this.cdr.detectChanges();
    }
}