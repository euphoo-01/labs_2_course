function extend(...objects) {
	return Object.assign({}, ...objects);
}

console.log(extend({ a: 1, b: 2 }, { c: 3 }));
