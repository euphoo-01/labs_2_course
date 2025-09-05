function basicOperation(operation, value1, value2) {
	switch (operation) {
		case "+":
			return value1 + value2;
		case "-":
			return value1 - value2;
		case "*":
			return value1 * value2;
		case "/":
			return value2 === 0 ? "Нельзя делить на ноль" : value1 / value2;
		default:
			return "Неверный оператор.";
	}
}

console.log(basicOperation("+", 5, 10));
console.log(basicOperation("*", 5, 10));
console.log(basicOperation("-", 5, 10));
console.log(basicOperation("/", 5, 10));
