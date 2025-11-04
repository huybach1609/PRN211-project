import { User } from "./User";

export interface IStickyNote {
    id: number;
    userId: number;
    name?: string;
    details?: string;
    user?: User;
}
export class StickyNote implements IStickyNote {
    constructor(
        public id: number,
        public userId: number,
        public name?: string,
        public details?: string,
        public user?: User,
    ) {
    }
}
