import type { Provider } from './provider';
import type { Guid } from './guid';


export type Company = {
    id: Guid;
    name: string;
    providers: Provider[];
};