const car = {
	make: "Toyota",
	model: "Corolla",
	year: 1990,
};

console.log(car);

car.year = 2000;

console.log(car);

Object.defineProperties(car, {
	make: {
		writable: false,
		configurable: false,
	},
	model: {
		writable: false,
		configurable: false,
	},
	year: {
		writable: false,
		configurable: false,
	},
});

car.model = "AE86";
car.year = 1996;
console.log(car);
