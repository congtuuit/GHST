import { TagState } from "@/interface/layout/tagsView.interface";
import { UserState } from "@/interface/user/user";
import { GlobalState } from "./global.store";
import { IOrderState } from "@/features/order/orderSlice";

export interface IStore {
    user: UserState;
    tagsView: TagState;
    global: GlobalState;
    order: IOrderState;
}