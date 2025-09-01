export interface TruckDTO {
    id: number;
    model: number;
    chassisNumber: string;
    color: string;
    yearManufacture: number;
    plant: number;
}

export interface CreateTruckDTO {
    model: number;
    chassisNumber: string;
    color: string;
    yearManufacture: number;
    plant: number;
}
