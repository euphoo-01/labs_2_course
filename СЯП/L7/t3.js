function Book(title, author) {
	this.title = title;
	this.author = author;
	this.getTitle = function () {
		console.log(`Название книги: ${this.title}`);
	};
	this.getAuthor = function () {
		console.log(`Автор книги: ${this.author}`);
	};
}

const someBook = new Book("Путь мага", "Пауло Коэльо");
someBook.getAuthor();
someBook.getTitle();
