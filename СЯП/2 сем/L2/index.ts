const enum ProductCategory {
    Food = "Food",
    Tech = "Tech",
    Tools = "Tools",
}

class Product {
    public readonly id: string;
    public name: string;
    public price: number;
    public description?: string;
    public category: ProductCategory;

    constructor(data: NewProduct) {
        const { name, price, category, description } = data;
        this.id = Math.random().toString(36).slice(2);
        this.name = name;
        this.price = price;
        this.category = category;
        this.description = description;
    }

    public getInfo(): string {
        const desc = this.description ? ` (${this.description})` : "";
        return `[${this.id}] ${this.name}${desc} | Категория: ${this.category} | Цена: $${this.price}`;
    }
}

type ProductUpdate = Partial<Product>;
type NewProduct = Omit<Product, "id" | "getInfo">;

class Catalog {
    public products: Product[];

    constructor() {
        this.products = [];
    }

    public addProduct(product: Product): void {
        this.products.push(product);
    }

    public removeProduct(id: string): void {
        this.products = this.products.filter((product) => product.id !== id);
    }

    public getProductById(id: string): Product | undefined {
        return this.products.find((product) => product.id === id);
    }

    public getAllProducts(): Product[] {
        return this.products;
    }

    public getProductsByCategory(category: ProductCategory): Product[] {
        return this.products.filter((product) => product.category === category);
    }
}

class Order<T extends Product> {
    public readonly id: string;
    public products: T[];
    public totalPrice: number;
    public customerId: string;

    constructor(products: T[], customerId: string) {
        this.id = `ORD-${Math.random().toString(36).substring(2, 9)}`;
        this.products = products;
        this.customerId = customerId;
        this.totalPrice = this.calculateTotalPrice();
    }

    public calculateTotalPrice(): number {
        return this.products.reduce((sum, product) => sum + product.price, 0);
    }

    public getOrderInfo(): string {
        return `Заказ #${this.id} | Клиент: ${this.customerId} | Товаров: ${this.products.length} | Итого: $${this.totalPrice}`;
    }
}

type OrderSummary = Pick<Order<Product>, "id" | "totalPrice">;

class Customer {
    public readonly id: string;
    public name: string;
    public email: string;

    constructor(name: string, email: string) {
        this.id = `C-${Math.random().toString(36).slice(2)}`;
        this.name = name;
        this.email = email;
    }

    public getCustomerInfo(): string {
        return `Клиент: ${this.name} (${this.email}) | ID: ${this.id}`;
    }
}

class OrderManager {
    public orders: Order<Product>[];

    constructor() {
        this.orders = [];
    }

    public createOrder(
        customer: Customer,
        products: Product[],
    ): Order<Product> {
        const newOrder = new Order<Product>(products, customer.id);
        this.orders.push(newOrder);
        return newOrder;
    }

    public getOrderById(id: string): Order<Product> | undefined {
        return this.orders.find((order) => order.id === id);
    }

    public getAllOrders(): Order<Product>[] {
        return this.orders;
    }

    public getOrdersByCustomer(customerId: string): Order<Product>[] {
        return this.orders.filter((order) => order.customerId === customerId);
    }
}

const apple = new Product({
    name: "Яблоко",
    price: 2,
    category: ProductCategory.Food,
    description: "Свежее зеленое яблоко",
});
const laptop = new Product({
    name: "Ноутбук",
    price: 1500,
    category: ProductCategory.Tech,
    description: "Мощный игровой ноутбук",
});
const hammer = new Product({
    name: "Молоток",
    price: 15,
    category: ProductCategory.Tools,
});
const beef = new Product({
    name: "Вяленая говядина",
    price: 1,
    category: ProductCategory.Food,
});

console.log(apple.getInfo());
console.log(laptop.getInfo());
console.log(hammer.getInfo());

const catalog = new Catalog();
catalog.addProduct(apple);
catalog.addProduct(laptop);
catalog.addProduct(hammer);
catalog.addProduct(beef);
console.log(`Всего товаров в каталоге: ${catalog.getAllProducts().length}`);

const foodProducts = catalog.getProductsByCategory(ProductCategory.Food);
console.log("Товары в категории Food:");
foodProducts.forEach((p) => console.log(` - ${p.name}`));

catalog.removeProduct(hammer.id);
console.log(
    `Товаров после удаления молотка: ${catalog.getAllProducts().length}`,
);

const foundProduct = catalog.getProductById(laptop.id);
console.log(`Найден товар по ID: ${foundProduct?.name}`);

const customer1 = new Customer("Джефри Эпштейн", "jeff@epstein.com");
const customer2 = new Customer("Дмитрий Шиман", "dmitry@shiman.com");

console.log(customer1.getCustomerInfo());
console.log(customer2.getCustomerInfo());

const orderManager = new OrderManager();

const order1 = orderManager.createOrder(customer1, [beef]);
const order2 = orderManager.createOrder(customer2, [apple, laptop]);

console.log(order1.getOrderInfo());
console.log(order2.getOrderInfo());

const jeffOrders = orderManager.getOrdersByCustomer(customer1.id);
console.log(`Количество заказов Ивана: ${jeffOrders.length}`);
console.log(`Сумма первого заказа Ивана: $${jeffOrders[0].totalPrice}`);

const newDraft: NewProduct = {
    name: "Смартфон",
    price: 800,
    category: ProductCategory.Tech,
};

const newProduct = new Product(newDraft);
const orderSummary: OrderSummary = {
    id: order1.id,
    totalPrice: order1.totalPrice,
};
console.log("Сводка заказа:", orderSummary);
