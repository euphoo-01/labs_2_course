import "./style.css";
import { TodoList } from "./entities/todolist.js";
import { initAddForm } from "./modules/addForm.js";
import { initFilters } from "./modules/filters.js";
import { initTodoListRendering } from "./modules/renderTodoList.js";

const root = document.querySelector("#root");

const myTodoList = new TodoList("Дейлики");

const renderApp = initTodoListRendering(root, myTodoList);

initAddForm(root, myTodoList, renderApp);
initFilters(root, myTodoList, renderApp);
