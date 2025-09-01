const Weekdays = ["пн", "вт", "ср", "чт", "пт", "сб", "вс"];
const input = prompt("Введите номер дня недели: ");
if (input > 0 && input < 8) {
	alert(Weekdays.at(input - 1));
} else {
	alert("Некорректный тип данных!");
}
