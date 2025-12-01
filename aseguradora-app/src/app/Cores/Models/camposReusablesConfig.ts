export interface CampoConfig {
  key: string;
  label?: string;
  type?: 'text' | 'number' | 'email' | 'date'| 'actions';
  required?: boolean;
  minLength?: number;
  maxLength?: number;
  hidden?: boolean;
  disabled?: boolean;
}