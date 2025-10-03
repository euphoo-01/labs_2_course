const rectangle = {
	_width: 0,
	_height: 0,
	get area() {
		return this._height * this._width;
	},
	get width() {
		return this._width;
	},
	set width(val) {
		this._width = val;
	},
	set height(val) {
		this._height = val;
	},
};

rectangle.height = 100;
rectangle.width = 95;
console.log(rectangle.area);
