import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../Envairoments/environment";
import { Injectable } from "@angular/core";
import { Seguro } from "../Models/seguros.model";

@Injectable({ providedIn: 'root' })
export class SegurosService {

  private url = environment.apiUrl;
  private end = environment.endpoints.seguros;
  constructor(private http: HttpClient) {}

  //servicio para obtener todos los seguros
  obtenerSeguros() {
    return this.http.get<Seguro[]>(`${this.url}${this.end.obtenerSeguros}`);
  }
  //servicio para obtener asegurados por codigo de seguro al endpoint
  obtenerAseguradosPorCodigoSeguro(codigoSeguro: string) {
    return this.http.get(`${this.url}${this.end.consultarPorCodigo}/${codigoSeguro}`);
  }

  //servicio para insertar un nuevo seguro
  insertarSeguro(seguro: Seguro, usuario: string) {
    const params = new HttpParams().set('usuario', usuario);
    return this.http.post(`${this.url}${this.end.insertar}`, seguro, { params });
  }

  //servicio para actualizar un seguro existente
  actualizarSeguro(seguro: Seguro, usuario: string) {
    const params = new HttpParams().set('usuario', usuario);
    return this.http.put(`${this.url}${this.end.actualizar}`, seguro, { params });
  }

  //servicio para eliminar un seguro
  eliminarSeguro(id: number) {
    return this.http.delete(`${this.url}${this.end.eliminar}/${id}`);
  }

  //servicio para insertar seguros de forma masiva
  insertarSegurosMasivo(seguros: Seguro[], usuario: string) {
    const params = new HttpParams().set('usuario', usuario);
    return this.http.post(`${this.url}${this.end.insertarMasivo}`, seguros, { params });
  }
}
