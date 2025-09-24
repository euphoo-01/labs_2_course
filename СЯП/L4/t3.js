let storage = new Map();

const addItem = (name, count, price) => {
	storage.set(storage.size + 1, { name, count, price });
};

const delItemByID = (id) => {
	storage.delete(id);
};

const delItemByName = (name) => {
	storage.forEach((val, key) => {
		val.name === name && storage.delete(key);
	});
};

const changeCount = (id, count) => {
	storage.forEach((val, key) => key === id && (val.count = count));
};

const changePrice = (id, price) => {
	storage.forEach((val, key) => key === id && (val.price = price));
};

const sumPrice = () => {
	return Array.from(storage.entries()).reduce(
		(acc, entry) => (acc += entry[1].price * entry[1].count),
		0
	);
};

const countEntries = () => {
	return storage.size;
};

addItem("Морковка", 10, 5);
addItem("Груша", 100, 2);
addItem("Бульба", 10000, 1);

console.log(storage);
delItemByName("Морковка");
console.log(storage);
changeCount(3, 1000000);
console.log(storage);
console.log(sumPrice());
console.log(countEntries());
