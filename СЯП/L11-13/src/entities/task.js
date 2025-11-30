export class Task {
	#id;
	#title;
	#isCompleted;
	constructor(title) {
		this.#id = Symbol();
		this.#title = title;
		this.#isCompleted = false;
	}

	get id() {
		return this.#id;
	}

	get title() {
		return this.#title;
	}

	get isCompleted() {
		return this.#isCompleted;
	}

	toggleCompleteTask = () => {
		this.#isCompleted = !this.#isCompleted;
	};

	editTitle = (newTitle) => {
		this.#title = newTitle;
	};
}
