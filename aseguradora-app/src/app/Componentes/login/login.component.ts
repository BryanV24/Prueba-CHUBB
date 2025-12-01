import { Component } from '@angular/core';
import Swal from 'sweetalert2';
import { LoginService } from './loginService.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../Services/auth';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule,],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class LoginComponent {

  constructor(
    private loginService: LoginService,
    private router: Router,
    private authService: AuthService
  ) {}

  modo: 'login' | 'registro' = 'login';

  login = {
    correo: '',
    clave: ''
  };

  registro = {
    nombreCompleto: '',
    correo: '',
    clave: '',
    confirmar: ''
  };

  cambiarModo() {
    this.modo = this.modo === 'login' ? 'registro' : 'login';
  }

  //METODO PARA REGISTRAR UN USUARIO
  registrarUsuario() {
    if (this.registro.clave !== this.registro.confirmar) {
      Swal.fire({
        title: 'Advertencia',
        text: 'Las contraseñas no coinciden.',
        icon: 'warning'
      });
      return;
    }
    this.loginService.registrarUsuario(this.registro).subscribe({
      next: () => {
        Swal.fire({
          title: 'Éxito',
          text: 'Usuario registrado exitosamente.',
          icon: 'success'
          });
        this.cambiarModo();
      },
      error: err => {
        console.error('Error al registrar usuario:', err);
        Swal.fire({
          title: 'Error',
          text: 'Error al registrar usuario. Intenta nuevamente.',
          icon: 'error'
        });
      }
    });
  }

  //METODO PARA INICIAR SESION DE UN USUARIO
  iniciarSesion() {
    this.loginService.loginUsuario(this.login).subscribe({
      next: (usuario) => {
        this.authService.guardarSesion(usuario);
        Swal.fire({
          title: 'Éxito',
          text: 'Inicio de sesión exitoso.',
          icon: 'success',
          confirmButtonText: 'Continuar'
          }).then((result) => {
            if (result.isConfirmed) {
              this.router.navigate(['/navegadorMenu']);
            }
          });
      },
      error: (error) => {
        console.error('Error al iniciar sesión:', error);
        Swal.fire({
          title: 'Error',
          text: 'Credenciales incorrectas. Intenta nuevamente.',
          icon: 'error'
        });
      }
    });
  }
}
