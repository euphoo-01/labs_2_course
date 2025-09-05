const arr = [1, [1, 2, [3, 4]], [2, 4]];
const flatten = (arr) =>
	arr.reduce(
		(acc, val) =>
			Array.isArray(val) ? acc.concat(flatten(val)) : acc.concat(val),
		[]
	);

console.log(flatten(arr));
