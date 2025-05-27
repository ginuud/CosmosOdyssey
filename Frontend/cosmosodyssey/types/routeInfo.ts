import type {Planet} from './planet';
export type Guid = string;

export type RouteInfo = {
    id: Guid;
    from: Planet;
    to: Planet; 
    distance: number;
    reservationId: number | null; 
};