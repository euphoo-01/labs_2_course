const renderFilters = () => {
	const filtersWrapper = document.createElement("section");
	filtersWrapper.className = "filters-wrapper";

	const showAllButton = document.createElement("button");
	showAllButton.textContent = "Показать все";

	const showCompletedButton = document.createElement("button");
	showCompletedButton.textContent = "Показать выполненные";

	const showUncompletedButton = document.createElement("button");
	showUncompletedButton.textContent = "Показать невыполненные";

	const buttons = [showAllButton, showCompletedButton, showUncompletedButton];
	filtersWrapper.append(...buttons);

	return { filtersWrapper, buttons };
};

export const initFilters = (root, todolist, rerender) => {
	const { filtersWrapper, buttons } = renderFilters();

	buttons[0].addEventListener("click", () => {
		rerender(todolist.filterTasks());
	});
	buttons[1].addEventListener("click", () => {
		rerender(todolist.filterTasks(true));
	});
	buttons[2].addEventListener("click", () => {
		rerender(todolist.filterTasks(false));
	});

	root.append(filtersWrapper);
};
