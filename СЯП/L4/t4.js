let cache = new WeakMap();

const calc = (obj) => {
	if (!cache.has(obj)) {
		const result = Math.pow(obj.a * obj.b * obj.c, 1);
		console.log("Cache");
		cache.set(obj, result);
	}

	return cache.get(obj);
};

const obj = { a: 1, b: 2, c: 3 };
console.log(calc(obj));

console.log(calc(obj));
