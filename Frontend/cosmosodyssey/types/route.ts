export type Guid = string;

export type Route = {
    priceListId: Guid;
    routeInfoIds: Guid[];
    from: string;
    to: string;
    providerIds: Guid[];
    companyNames: string[];
    price: number;
    distance: number;
    travelTime: number;
}
