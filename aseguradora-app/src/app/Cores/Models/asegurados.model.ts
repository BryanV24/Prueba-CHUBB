export interface Asegurado {
  aseguradoId: number;
  cedula: string;
  nombreCompleto: string;
  telefono?: string;
  edad?: number;
  seguros?: any[];
}