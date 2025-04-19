import type { Leg } from './leg';
export type Guid = string;

export type PriceList = {
    id: Guid;
    validUntil: Date;
    legs: Leg[];
};