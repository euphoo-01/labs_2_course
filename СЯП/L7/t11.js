const user = {
	_firstName: "",
	_lastName: "",
	get fullName() {
		return `${this._firstName} ${this._lastName}`;
	},
	set fullName(val) {
		const parts = val.split(" ");
		if (parts.length !== 2) {
			console.error("Укажите только имя фамилию!");
		} else {
			this._firstName = parts[0];
			this._lastName = parts[1];
		}
	},
};

user.fullName = "Михаил Зубенко";
console.log(user.fullName);
