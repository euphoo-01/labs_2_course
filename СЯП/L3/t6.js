function tower(n) {
	const result = [];
	for (let i = 0; i < n; i++) {
		const spaces = " ".repeat(n - i - 1);
		const stars = "*".repeat(2 * i + 1);
		result.push(spaces + stars + spaces);
	}
	return result;
}

console.log(tower(5).join("\n"));
