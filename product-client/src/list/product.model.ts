export interface Product {
    id: string,
    name: string,
    category: {name: string},
    price: number,
    stockQuantity: number,
}