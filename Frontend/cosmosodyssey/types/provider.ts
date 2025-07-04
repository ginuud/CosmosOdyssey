import type { Leg } from './leg';
import type { Company } from './company';
import type { Guid } from './guid';


export type Provider = {
    id: Guid;
    company: Company;
    companyId: Guid;
    price: number;
    flightStart: Date;
    flightEnd: Date;
    leg: Leg;
    legId: Guid;
}