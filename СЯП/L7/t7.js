const circle = {
	_radius: 100,

	get radius() {
		return this._radius;
	},

	set radius(value) {
		if (value > 0) {
			this._radius = value;
		}
	},

	get area() {
		return Math.PI * Math.pow(this._radius, 2);
	},
};

console.log(circle.radius);
circle.radius = 2;
console.log(circle.radius);
console.log(circle.area.toFixed(2));
