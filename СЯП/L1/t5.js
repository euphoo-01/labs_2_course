const prepod = "Смелов Владимир Владиславович";
const FIO = prepod.toLowerCase().split(" ");

const input = prompt("Введите ФИО");
const inputFIO = input.toLowerCase().split(" ");

if (
	(inputFIO[0] === FIO[0] && inputFIO[1] === FIO[1]) ||
	(inputFIO[1] === FIO[1] && inputFIO[2] === FIO[2]) ||
	(inputFIO[0] === FIO[0] && inputFIO.length === 1)
) {
	alert("Данные верны! Congrats!");
} else {
	alert("Отчислен!");
}
