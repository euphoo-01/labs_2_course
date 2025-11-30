import { Task } from "./task.js";

export class TodoList {
	#id;
	#title;
	#tasks;

	constructor(title, tasks = []) {
		this.#id = Symbol();
		this.#title = title;
		this.#tasks = tasks;
	}

	get tasks() {
		return [...this.#tasks];
	}

	addTask = (title) => {
		const newTask = new Task(title);
		this.#tasks = [...this.#tasks, newTask];
	};

	delTask = (id) => {
		this.#tasks = this.#tasks.filter((el) => el.id !== id);
	};

	filterTasks = (isCompleted) => {
		if (isCompleted !== undefined) {
			return this.#tasks.filter((el) => el.isCompleted === isCompleted);
		} else {
			return this.#tasks;
		}
	};
}
