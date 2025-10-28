function parallelepiped(h) {
	return function (a) {
		return function (b) {
			return a * b * h;
		};
	};
}

let V10 = parallelepiped(10);

console.log(V10(5)(5));
