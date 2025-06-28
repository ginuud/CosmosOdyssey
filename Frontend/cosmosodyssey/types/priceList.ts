import type { Leg } from './leg';
import type { Guid } from './guid';

export type PriceList = {
    id: Guid;
    validUntil: Date;
    legs: Leg[];
};