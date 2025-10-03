let arr = [1, 2, 3];
Object.defineProperty(arr, "sum", {
	get() {
		return this.reduce((acc, val) => acc + val, 0);
	},
	configurable: false,
});

console.log(arr.sum);
