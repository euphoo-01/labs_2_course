function reverse(str) {
	return str
		.split("")
		.reverse()
		.filter((ch) => /[a-zA-Z]/)
		.join("");
}

console.log(reverse("Hello world!"));
