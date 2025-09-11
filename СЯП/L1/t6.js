const math = 4;
const rus = 2;
const eng = 4;

if (math > 3 && rus > 3 && eng > 3) {
	console.log("Ура! Поздравляем с переходом на следующий курс.");
} else if (math > 3 || rus > 3 || eng > 3) {
	console.log("Почти получилось. Пересдайте и попробуйте еще");
} else {
	console.error("Отчислен");
}
