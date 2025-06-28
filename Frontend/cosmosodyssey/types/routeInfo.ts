import type {Planet} from './planet';
import type {Guid} from './guid';

export type RouteInfo = {
    id: Guid;
    from: Planet;
    to: Planet; 
    distance: number;
};