const Shape = {
	color: "white",
	type: "shape",
	describe() {
		return `Это ${this.color} ${this.type}.`;
	},
};

const Square = Object.create(Shape);
Square.type = "квадрат";
Square.form = "прямоугольная";

const largeSquare = Object.create(Square);
largeSquare.color = "желтый";
largeSquare.size = "большой";
largeSquare.sideLength = 100;

const smallSquare = Object.create(largeSquare);
smallSquare.size = "маленький";
smallSquare.sideLength = 50;

const Circle = Object.create(Shape);
Circle.type = "круг";
Circle.form = "круглая";

const greenCircle = Object.create(Circle);
greenCircle.color = "зеленый";
greenCircle.radius = 80;

const whiteCircle = Object.create(Circle);
whiteCircle.radius = 60;

const Triangle = Object.create(Shape);
Triangle.type = "треугольник";
Triangle.form = "треугольная";

const triangleWithOneLine = Object.create(Triangle);
triangleWithOneLine.lines = ["одна линия посередине"];

const triangleWithThreeLines = Object.create(triangleWithOneLine);
triangleWithThreeLines.lines = [
	...triangleWithOneLine.lines,
	"две линии по бокам",
];

console.log("\nСвойства зеленого круга:");
console.log(greenCircle);
console.log(`- Цвет: ${greenCircle.color}`);
console.log(`- Радиус: ${greenCircle.radius}`);
console.log(`- Тип: ${greenCircle.type} (унаследовано от Circle)`);

console.log("\nСвойства треугольника с тремя линиями:");
console.log(triangleWithThreeLines);
console.log(`- Линии: ${triangleWithThreeLines.lines.join(", ")}`);
console.log(`- Цвет: ${triangleWithThreeLines.color} (унаследовано от Shape)`);

const hasOwnColorProperty = smallSquare.hasOwnProperty("color");

console.log(
	`Имеет ли маленький квадрат собственное свойство 'color'? - ${hasOwnColorProperty}`
);
console.log(
	`(Цвет "${smallSquare.color}" унаследован от объекта 'largeSquare')`
);

console.log(
	"Прототип largeSquare в smallSquare:",
	Object.getPrototypeOf(smallSquare) === largeSquare
);
console.log(
	"Прототип Square в largeSquare:",
	Object.getPrototypeOf(largeSquare) === Square
);
console.log(
	"Прототип Shape в Square :",
	Object.getPrototypeOf(Square) === Shape
);
