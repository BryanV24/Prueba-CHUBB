import { ChangeDetectorRef, Component, EventEmitter, Input, Output, ViewChild } from "@angular/core";
import { CampoConfig } from "../../../Cores/Models/camposReusablesConfig";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { SegurosComponent } from "../componenteSeguroModal/seguros.component";
import { SegurosService } from "../../../Cores/Servicios/seguros.service";
import { Seguro } from "../../../Cores/Models/seguros.model";
import * as XLSX from 'xlsx';
import Swal from "sweetalert2";

@Component({
    selector: 'app-variables-reusables-seguros',
    standalone: true,
    imports: [FormsModule, CommonModule, SegurosComponent],
    templateUrl: './variablesReusablesSeguros.component.html'
})
export class VariablesReusablesSegurosComponent {
  
  constructor(
    private segurosServicio: SegurosService,
    private cdr: ChangeDetectorRef
  ) {}
  @Input() campos: CampoConfig[] = [];
  @Input() datos: any[] = [];
  @Output() onEditar = new EventEmitter<any>();
  @Output() onEliminar = new EventEmitter<any>();
  @ViewChild(SegurosComponent) segurosComponente!: SegurosComponent;
  formModel: any = {};
  mostrarModal = false;
  actualizar = false;
  archivoSeleccionado: File | null = null;
  listaSeguros: Seguro[] = [];
  paginaActual: number = 1;
  registrosPorPagina: number = 5;
  totalPaginas: number = 0;

  ngOnInit() {
    this.cargarSeguros();
  }
//METODO PARA PODER CONSULTAR TODOS LOS SEGUROS
  cargarSeguros() {
    this.segurosServicio.obtenerSeguros().subscribe({
        next: (resp: Seguro[]) => {
          this.datos = resp;
          this.totalPaginas = Math.ceil(this.datos.length / this.registrosPorPagina) || 1;
          this.paginaActual = 1;
          this.cdr.detectChanges();
        },
        error: err => console.error('Error al cargar seguros', err)
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
        this.listaSeguros = this.procesarTxt(text);
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
        this.listaSeguros = jsonData.map(row => ({
          seguroId: 0,
          nombreSeguro: String(row['nombreSeguro'] || row['NombreSeguro'] || row['nombre'] || '').trim(),
          codigoSeguro: String(row['codigoSeguro'] || row['CodigoSeguro'] || row['codigo'] || '').trim(),
          sumaAsegurada: Number(row['sumaAsegurada'] || row['SumaAsegurada'] || 0),
          prima: Number(row['prima'] || row['Prima'] || 0)
        }));

        this.guardarMasivo();
      };
      reader.readAsArrayBuffer(this.archivoSeleccionado);
    }
  }
  
  //METODO PARA PROCESAR LOS CAMPOS DENTRO DE LOS ARCHIVOS TXT
  procesarTxt(text: string): Seguro[] {
    const lineas = text.split('\n').filter(l => l.trim() !== '');
    return lineas.map(linea => {
      const obj: any = {};
      const pares = linea.split(','); // separar cada "clave:valor"
      pares.forEach(par => {
        const [clave, valor] = par.split(':').map(s => s.trim());
        obj[clave] = valor;
      });

      return {
        seguroId: 0,
        nombreSeguro: obj['nombreSeguro'] || '',
        codigoSeguro: obj['codigoSeguro'] || '',
        sumaAsegurada: Number(obj['sumaAsegurada'] || 0),
        prima: Number(obj['prima'] || 0)
      };
    });
  }
  
  
  //METODO PARA GUARDAR LOS ASEGURADOS DE FORMA MASIVA
  guardarMasivo() {
    if (!this.listaSeguros.length) return;

    const usuario = "Bryan Vera" ;
    this.segurosServicio.insertarSegurosMasivo(this.listaSeguros, usuario)
      .subscribe({
        next: () => {
          Swal.fire({
            title: 'Creado',
            text: 'Carga de Seguros de manera masiva fue exitosa',
            icon: 'success',
            showConfirmButton: true,
            allowOutsideClick: false,
            allowEscapeKey: false
          });
          this.cargarSeguros();
        },
        error: err => console.error('Error al guardar masivo', err)
      });
  }
  
  //EVENTOS PARA ENVIAR LOS DATOS DE EDITAR Y ELIMINAR SEGUROS
  editar(seguro: any) {
    this.onEditar.emit(seguro);
  }  
  eliminar(seguro: any) {
    this.onEliminar.emit(seguro);
  }

  //METODO PARA ABRIR EL MODAL DE AGREGAR SEGURO
  abrirModal() {
    this.formModel = {};
    this.mostrarModal = true;
    this.actualizar = false;
    this.cdr.detectChanges();
  }

  //METODO PARA CERRAR EL MODAL
  cerrarModal() {
    this.mostrarModal = false;
    this.formModel = null;
    this.cargarSeguros();
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