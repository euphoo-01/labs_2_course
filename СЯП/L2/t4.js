//TODO !

function reverse(str) {
	return str
		.split("")
		.reverse()
		.filter((ch) => /[a-zA-Z\ ]/.test(ch))
		.join("");
}

console.log(reverse("Hello world!"));
