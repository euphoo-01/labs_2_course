let store = {
	state: {
		profilePage: {
			posts: [
				{ id: 1, message: "Hi", likesCount: 12 },
				{ id: 2, message: "Bye", likesCount: 1 },
			],
			newPostText: "About me",
		},
		dialogsPage: {
			dialogs: [
				{ id: 1, name: "Valera" },
				{ id: 2, name: "Andrey" },
				{ id: 3, name: "Sasha" },
				{ id: 4, name: "Viktor" },
			],
			messages: [
				{ id: 1, message: "hi" },
				{ id: 2, message: "hi hi" },
				{ id: 3, message: "hi hi hi" },
			],
		},
		sidebar: [],
	},
};

const {
	state: {
		profilePage: { posts },
		dialogsPage: { dialogs, messages },
	},
} = store;

posts.forEach((el) => console.log(el.likesCount));

console.log(dialogs.filter((el) => !(el.id % 2)));

messages.map((el) => (el.message = "Hello user"));
console.log(messages);
