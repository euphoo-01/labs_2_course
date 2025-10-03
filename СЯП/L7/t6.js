const item = { price: 1000 };

console.log(Object.getOwnPropertyDescriptor(item, "price"));

Object.defineProperty(item, "price", {
	writable: false,
	configurable: false,
});
console.log(Object.getOwnPropertyDescriptor(item, "price"));
