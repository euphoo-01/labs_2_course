const team = {
	players: [
		{ fullName: "Стефен Карри", age: 37, role: "Разыгрывающий защитник" },
		{ fullName: "Деррик Роуз", age: 36, role: "Разыгрывающий защитник" },
		{ fullName: "Шакил О'нил", age: 53, role: "Центровой" },
		{ fullName: "Майкл Джордан", age: 62, role: "Атакующий защитник" },
	],
	showPlayers: function () {
		this.players.forEach((player) =>
			console.log(
				`Полное имя: ${player.fullName}, возраст: ${player.age}, игровая роль: ${player.role}`
			)
		);
	},
};

team.showPlayers();
