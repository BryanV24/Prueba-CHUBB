import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private usuarioKey = 'usuarioSesion';

  guardarSesion(usuario: any) {
    localStorage.setItem(this.usuarioKey, JSON.stringify(usuario));
  }

  obtenerUsuario() {
    const data = localStorage.getItem(this.usuarioKey);
    return data ? JSON.parse(data) : null;
  }

  cerrarSesion() {
    localStorage.removeItem(this.usuarioKey);
  }

  estaLogueado(): boolean {
    return !!this.obtenerUsuario();
  }

}
