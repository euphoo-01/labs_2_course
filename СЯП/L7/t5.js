const counter = (function () {
	let count = 0;

	return {
		increment() {
			return ++count;
		},
		decrement() {
			return --count;
		},
		getCount() {
			return count;
		},
	};
})();

console.log(counter.increment());
console.log(counter.increment());
console.log(counter.decrement());
console.log(counter.getCount());
