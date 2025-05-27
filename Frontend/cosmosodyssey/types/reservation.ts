import type { RouteInfo } from './routeInfo';

export type Guid = string;

export type Reservation = {
    id: number;
    firstName: string;
    lastName: string;
    // routes: RouteInfo[];
    routeInfoIds: Guid[];
    totalQuotedPrice: number;
    totalQuotedTravelTime: number;
    transportationCompanyNames: string[];
}