let exit = 0;

function* moveObject(x = 0, y = 0) {
	while (true) {
		let direction = prompt("Введите команду (left, right, up, down):");

		for (let i = 0; i < 10; i++) {
			switch (direction) {
				case "left":
					x--;
					break;
				case "right":
					x++;
					break;
				case "up":
					y++;
					break;
				case "down":
					y--;
					break;
				case "exit":
					exit = 1;
					break;
				default:
					console.log("Неизвестная команда!");
			}
			yield { x, y };
		}
	}
}

let gen = moveObject(0, 0);

while (exit === 0) {
	console.log(gen.next().value);
}
