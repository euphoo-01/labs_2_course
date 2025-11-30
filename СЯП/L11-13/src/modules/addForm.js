const renderAddForm = () => {
	const addWrapper = document.createElement("section");
	addWrapper.className = "add-form";

	const addInput = document.createElement("input");
	addInput.className = "add-form-input";
	addInput.type = "text";
	addInput.placeholder = "Введите название таски:";

	const addButton = document.createElement("button");
	addButton.textContent = "Добавить";

	addWrapper.append(addInput, addButton);

	return { addWrapper, addInput, addButton };
};

export const initAddForm = (root, todolist, rerender) => {
	const { addWrapper, addInput, addButton } = renderAddForm();

	addButton.addEventListener("click", () => {
		const title = addInput.value.trim();
		if (title) {
			todolist.addTask(title);
			addInput.value = "";
			rerender();
		}
	});

	root.append(addWrapper);
};
