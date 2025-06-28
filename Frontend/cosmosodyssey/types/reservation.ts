import type { RouteInfo } from './routeInfo';
import type { Guid } from './guid';

export type Reservation = {
    id: number;
    firstName: string;
    lastName: string;
    routes?: RouteInfo[]; // kasutan GET meetodi jaoks ja frontis kuvamiseks
    routeInfoIds?: Guid[]; //kasutan POST meetodi jaoks
    totalQuotedPrice: number;
    totalQuotedTravelTime: number;
    transportationCompanyNames: string[];
}