import { HttpClient } from "@angular/common/http";
import { environment } from "../../Envairoments/environment";
import { Injectable } from "@angular/core";
import { AsignacionSeguro } from "../Models/asignarSegurosAsegurados.model";

@Injectable({ providedIn: 'root' })
export class AsignacionService {

  private url = environment.apiUrl;
  private end = environment.endpoints.asignacion;

  constructor(private http: HttpClient) {}

  //servicio para asignar múltiples seguros a un asegurado
  asignarMultipleSeguros(data: AsignacionSeguro) {
    return this.http.post(`${this.url}${this.end.asignarSeguros}`, data);
  }
}
