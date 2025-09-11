const createString = (p1 = "Шиман", p2, p3) => {
	console.log(p1 + p2 + p3);
};
createString(undefined, "Дмитрий", prompt("Введите параметр 3: "));
