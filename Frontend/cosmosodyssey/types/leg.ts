import type {RouteInfo} from './routeInfo';
import type {Provider} from './provider';
import type {PriceList} from './priceList';
import type {Guid} from './guid';


export type Leg = {
    id: Guid;
    routeInfo: RouteInfo;
    routeInfoId: Guid;
    providers: Provider[];
    priceList: PriceList;
    priceListId: Guid;
}