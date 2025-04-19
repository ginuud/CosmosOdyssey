import type { Provider } from './provider';

export type Guid = string;

export type Company = {
    id: Guid;
    name: string;
    providers: Provider[];
};