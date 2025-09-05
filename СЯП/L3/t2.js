const flatten = (arr) =>
	arr.reduce(
		(acc, val) =>
			Array.isArray(val) ? acc.concat(flatten(val)) : acc.concat(val),
		[]
	);

const sum = (arr) => flatten(arr).reduce((acc, val) => acc + val, 0);

console.log(sum([1, 2, 3, 4, [5, [6, [7]]]]));
