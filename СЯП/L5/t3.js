var a = 10;
let b = 20;
const c = 30;

function hello() {
	return "Hello, world!";
}

console.log(window.a);
console.log(window.b);
console.log(window.c);
console.log(window.hello());

window.a = 1000000;

console.log("Новая а: ", window.a);

window.newVariable = "йцукен";
console.log(`Новая переменная: ${newVariable}`);
