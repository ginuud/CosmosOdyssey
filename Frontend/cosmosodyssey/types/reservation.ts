import type { Leg } from './leg';
import type { Company } from './company';

export type Guid = string;

export type Reservation = {
    id: number;
    firstName: string;
    lastName: string;
    // routes: RouteInfo[];
    routeIds: Guid[];
    totalQuotedPrice: number;
    totalQuotedTravelTime: number;
    transportationCompanyNames: string[];
}