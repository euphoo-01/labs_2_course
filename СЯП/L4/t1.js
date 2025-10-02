const products = new Set();

const add = (set, ...args) => {
	for (const arg of args) {
		set.add(arg);
	}
};
const del = (set, ...args) => {
	for (const arg of args) {
		set.delete(arg);
	}
};
const find = (set, prod) => {
	return set.has(prod);
};
const count = () => {
	return [...products.entries()].length;
};

add(products, "телефон", "часы", "айпад", "банан");
console.log(find(products, "телефон"));
console.log(products);
del(products, "телефон", "айпад");
console.log(products);
console.log(find(products, "телефон"));
console.log(count());
