import type { Guid } from './guid';

export type Route = {
    routeInfoIds: Guid[];
    from: string;
    to: string;
    providerIds: Guid[];
    companyNames: string[];
    price: number;
    distance: number;
    travelTime: number;
}
