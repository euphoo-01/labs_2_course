const renderTodoListItems = (tasks, todoListInstance, rerenderCallback) => {
	const todolistWrapper = document.createElement("ul");
	todolistWrapper.className = "todo-list-items";

	if (tasks && tasks.length > 0) {
		tasks.forEach((task) => {
			const wrapper = document.createElement("li");
			wrapper.id = `task-${task.id.toString()}`;
			if (task.isCompleted) {
				wrapper.classList.add("completed");
			}

			const title = document.createElement("h3");
			title.textContent = task.title;

			const completeButton = document.createElement("button");
			completeButton.className = "task-complete-button";
			completeButton.textContent = task.isCompleted ? "Сделано" : "Не сделано";
			completeButton.addEventListener("click", () => {
				task.toggleCompleteTask();
				rerenderCallback();
			});

			const editButton = document.createElement("button");
			editButton.className = "task-edit-button";
			editButton.textContent = "Редактировать";
			editButton.addEventListener("click", () => {
				const newTitle = prompt("Введите новое название задачи:", task.title);
				if (newTitle !== null && newTitle.trim() !== "") {
					task.editTitle(newTitle.trim());
					rerenderCallback();
				}
			});

			const delButton = document.createElement("button");
			delButton.className = "task-del-button";
			delButton.textContent = "Удалить";
			delButton.addEventListener("click", () => {
				todoListInstance.delTask(task.id);
				rerenderCallback();
			});

			wrapper.append(title, completeButton, editButton, delButton);
			todolistWrapper.append(wrapper);
		});
	} else {
		const noTasksMessage = document.createElement("li");
		noTasksMessage.textContent = "Задач пока нет.";
		todolistWrapper.append(noTasksMessage);
	}

	return todolistWrapper;
};

export const initTodoListRendering = (root, todoListInstance) => {
	const todolistContainer = document.createElement("section");
	todolistContainer.className = "todo-list-container";
	root.append(todolistContainer);

	const render = (tasksToRender = todoListInstance.tasks) => {
		todolistContainer.innerHTML = "";
		const listElement = renderTodoListItems(
			tasksToRender,
			todoListInstance,
			render
		);
		todolistContainer.append(listElement);
	};

	render();

	return render;
};
