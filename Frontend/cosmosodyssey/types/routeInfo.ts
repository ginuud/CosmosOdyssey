import type {Planet} from './planet';
export type Guid = string;

export type RouteInfo = {
    id: Guid;
    from: Planet;
    fromId: Guid;
    to: Planet; 
    toId: Guid; 
    distance: number;
};