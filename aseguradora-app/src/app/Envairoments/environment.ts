export const environment = {
  production: false,

  apiUrl: 'https://localhost:44339',

  endpoints: {
    autenticacion:{
      registrarUsuario: '/api/Autenticacion/RegistrarUsuario',
      login: '/api/Autenticacion/LoginUsuario'
    },
    seguros: {
      obtenerSeguros: '/api/Seguros/ObtenerSeguros',
      consultarPorCodigo: '/api/Seguros/Codigo',
      insertar: '/api/Seguros/InsertarSeguro',
      actualizar: '/api/Seguros/ActualizarSeguro',
      eliminar: '/api/Seguros/EliminarSeguro',
      insertarMasivo: '/api/Seguros/InsertarSegurosMasivo'
    },

    asegurados: {
      obtenerAsegurados: '/api/Asegurados/ObtenerAsegurados',
      consultarPorCedula: '/api/Asegurados/consultaSeguroPorCedula',
      insertar: '/api/Asegurados/InsertarAsegurado',
      actualizar: '/api/Asegurados/ActualizarAsegurado',
      eliminar: '/api/Asegurados/EliminarAsegurado',
      insertarMasivo: '/api/Asegurados/InsertarAseguradosMasivo'
    },

    asignacion: {
      asignarSeguros: '/api/AsignarSegurosAsegurados/AsignarSegurosAsegurados'
    }
  }
};
