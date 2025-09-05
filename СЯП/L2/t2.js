function cubes(n) {
	if (n === 1) {
		return 1;
	}
	if (n === 0) {
		return 0;
	}
	if (n < 0) {
		return 0;
	}
	return n * n * n + cubes(n - 1);
}

console.log(cubes(3));
