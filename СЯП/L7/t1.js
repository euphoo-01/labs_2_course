const person = {
	name: "Григорий",
	age: 20,
	greet: function () {
		return `Привет, ${this.name}`;
	},
	ageAfterYears: function (years) {
		return this.age + years;
	},
};

console.log(person.greet());
console.log(person.ageAfterYears(20));
