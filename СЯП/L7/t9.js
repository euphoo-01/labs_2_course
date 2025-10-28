let arr = [1, 2, 3];
Object.defineProperty(arr, "sum", {
	get() {
		return this.reduce((acc, val) => acc + val, 0);
	},
});

let arr2 = {
	_arr: [1, 2, 3],
	get arr() {
		return this._arr;
	},
	set arr(val) {
		if (Array.isArray(val)) {
			this._arr = val;
		} else if (typeof val === "number") {
			this._arr = [val];
		} else {
			throw new Error(
				"Неверное значение свойства. Можно передать только массив"
			);
		}
	},
	get sum() {
		return this._arr.reduce((acc, val) => (acc += val), 0);
	},
};

console.log(arr.sum);
console.log(arr2.sum);
arr2.arr = 1;
console.log(arr2.arr);
arr2.arr = [1, 2, 3, 4];
console.log(arr2.arr);
arr2.arr = "123";
