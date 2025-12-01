import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../Envairoments/environment";

@Injectable({ providedIn: 'root' })
export class LoginService {
  private url = environment.apiUrl;
  private end = environment.endpoints.autenticacion;
  constructor(private http: HttpClient) {}

  //SERVICIO PARA REGISTRAR UN USUARIO
  registrarUsuario(usuario: any) {
    return this.http.post(`${this.url}${this.end.registrarUsuario}`, usuario);
  }

  //SERVICIO PARA INICIAR SESION
  loginUsuario(credenciales: any) {
    return this.http.post(`${this.url}${this.end.login}`, credenciales);
  }
}