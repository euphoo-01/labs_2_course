function sr_arifm(array) {
	let sum = 0;
	array.forEach((el) => {
		sum += el;
	});
	return sum / array.length;
}

console.log(sr_arifm([1, 2, 3, 4, 5]));
