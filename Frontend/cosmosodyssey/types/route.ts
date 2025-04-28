export type Guid = string;

export type Route = {
    routeInfoId: Guid;
    from: string;
    to: string;
    providerId: Guid;
    companyName: string;
    price: number;
    distance: number;
    travelTime: number;
}
