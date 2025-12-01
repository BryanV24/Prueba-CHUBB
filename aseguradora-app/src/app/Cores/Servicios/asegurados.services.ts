import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../Envairoments/environment";
import { Injectable } from "@angular/core";
import { Asegurado } from "../Models/asegurados.model";

@Injectable({ providedIn: 'root' })
export class AseguradosService {

  private url = environment.apiUrl;
  private end = environment.endpoints.asegurados;

  constructor(private http: HttpClient) {}

  //servicio para obtener todos los asegurados
  obtenerAsegurados() {
    return this.http.get<Asegurado[]>(`${this.url}${this.end.obtenerAsegurados}`);
  }
  //servicio para obtener los seguros del asegurado por cédula
  obtenerSegurosPorCedula(cedula: string) {
    return this.http.get(`${this.url}${this.end.consultarPorCedula}/${cedula}`);
  }

  //servicio para insertar un nuevo asegurado
  insertarAsegurado(asegurados: any, usuario: string) {
    const params = new HttpParams().set('usuario', usuario);
    return this.http.post(`${this.url}${this.end.insertar}`, asegurados, {params});
  }

  //servicio para actualizar un asegurado existente
  actualizarAsegurado(asegurados: any, usuario: string) {
    const params = new HttpParams().set('usuario', usuario);
    return this.http.put(`${this.url}${this.end.actualizar}`, asegurados, {params});
  }

  //servicio para eliminar un asegurado
  eliminarAsegurado(aseguradosid: number) {
    return this.http.delete(`${this.url}${this.end.eliminar}/${aseguradosid}`);
  }

  //servicio para insertar asegurados de forma masiva
  insertarAseguradosMasivo(asegurados: Asegurado[], usuario: string) {
    const params = new HttpParams().set('usuario', usuario);
    return this.http.post(`${this.url}${this.end.insertarMasivo}`, asegurados, { params });
  }
}
