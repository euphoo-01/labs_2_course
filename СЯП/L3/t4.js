function chetoAscii(str) {
	const total1 = str
		.split("")
		.map((ch) => ch.charCodeAt(0))
		.join("");
	console.log(total1);
	const total2 = total1.replace(/7/g, "1");
	console.log(total2);
	return Number(total1) - Number(total2);
}

console.log(chetoAscii("ABC"));
