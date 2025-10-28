// 1

// T1
let user = {
	name: "Masha",
	age: 21,
};

let copyUser = { ...user };
console.log("T1:");
console.log(copyUser);

copyUser.name = "Sasha";
console.log(copyUser);
console.log(user);

// T2
let numbers = [1, 2, 3];
let copyNumbers = [...numbers];

console.log("T2:");

console.log(numbers);
console.log(copyNumbers);
copyNumbers.push(4, 5);
console.log(numbers);
console.log(copyNumbers);

// T3
let user1 = {
	name: "Masha",
	age: 23,
	location: {
		city: "Minsk",
		country: "Belarus",
	},
};
let copyUser1 = { ...user1, location: { ...user1.location } };

console.log("T3:");

console.log(user1);
console.log(copyUser1);

copyUser1.location.city = "Budapest";
copyUser1.location.city = "Hungary";

console.log(user1);
console.log(copyUser1);

// T4
let user2 = {
	name: "Masha",
	age: 28,
	skills: ["HTML", "CSS", "JavaScript", "React"],
};
let copyUser2 = { ...user2, skills: [...user2.skills] };

console.log("T4:");

console.log(user2);
console.log(copyUser2);

copyUser2.age = 16;
copyUser2.skills.pop("React");
copyUser2.skills.push("Angular");

console.log(user2);
console.log(copyUser2);

// T5
const array = [
	{ id: 1, name: "Vasya", group: 10 },
	{ id: 2, name: "Ivan", group: 11 },
	{ id: 3, name: "Masha", group: 12 },
	{ id: 4, name: "Petya", group: 10 },
	{ id: 5, name: "Kira", group: 11 },
];
const copyArray = [...array.map((el) => ({ ...el }))];

console.log("T5:");

console.log(array);
console.log(copyArray);

copyArray.push({ id: 6, name: "Altinbek", group: 9 });

console.log(array);
console.log(copyArray);

// T6
function clone(element) {
	if (typeof element !== "object" || element === null) {
		return element;
	}

	if (Array.isArray(element)) {
		return [...element.map((el) => clone(el))];
	}

	const _clone = { ...element };
	if (typeof element === "object") {
		for (const key in _clone) {
			if (typeof _clone[key] === "object") {
				_clone[key] = clone(_clone[key]);
			}
		}
	}

	return _clone;
}




let user4 = {
	name: "Masha",
	age: 19,
	studies: {
		university: "BSTU",
		speciality: "designer",
		year: 2020,
		exams: {
			maths: true,
			programming: false,
		},
	},
};

let cloneUser4 = clone(user4);

console.log("T6:");

console.log(user4);
console.log(cloneUser4);

cloneUser4.studies = {
	...cloneUser4.studies,
	university: "BSU",
	year: 2024,
	speciality: "software engineer",
};

console.log(user4);
console.log(cloneUser4);

// T7
let user5 = {
	name: "Masha",
	age: 22,
	studies: {
		university: "BSTU",
		speciality: "designer",
		year: 2020,
		department: {
			faculty: "FIT",
			group: 10,
		},
		exams: [
			{ maths: true, mark: 8 },
			{ programming: true, mark: 4 },
		],
	},
};
let cloneUser5 = clone(user5);

console.log("T7: ");
console.log(JSON.stringify(user5, null, 4));
console.log(JSON.stringify(cloneUser5, null, 4));

cloneUser5.studies = {
	...cloneUser5.studies,
	year: 2024,
	exams: [...cloneUser5.studies.exams, { economics: true, mark: 6 }],
};
console.log(JSON.stringify(user5, null, 4));
console.log(JSON.stringify(cloneUser5, null, 4));

// T8
let user6 = {
	name: "Masha",
	age: 21,
	studies: {
		university: "BSTU",
		speciality: "designer",
		year: 2020,
		department: {
			faculty: "FIT",
			group: 10,
		},
		exams: [
			{
				maths: true,
				mark: 8,
				professor: {
					name: "Ivan Ivanov",
					degree: "PhD",
				},
			},
			{
				programming: true,
				mark: 10,
				professor: {
					name: "Petr Petrov",
					degree: "PhD",
				},
			},
		],
	},
};

let cloneUser6 = clone(user6);

console.log("T8:");
console.log(JSON.stringify(user6, null, 4));
console.log(JSON.stringify(cloneUser6, null, 4));

// T9
let user7 = {
	name: "Masha",
	age: 20,
	studies: {
		university: "BSTU",
		speciality: "designer",
		year: 2020,
		department: {
			faculty: "FIT",
			group: 10,
		},
		exams: [
			{
				maths: true,
				mark: 8,
				professor: {
					name: "Ivan Petrov",
					degree: "PhD",
					articles: [
						{ title: "About HTML", pagesNumber: 3 },
						{ title: "About CSS", pagesNumber: 5 },
						{ title: "About JavaScript", pagesNumber: 1 },
					],
				},
			},
			{
				programming: true,
				mark: 10,
				professor: {
					name: "Petr Ivanov",
					degree: "PhD",
					articles: [
						{ title: "About HTML", pagesNumber: 3 },
						{ title: "About CSS", pagesNumber: 5 },
						{ title: "About JavaScript", pagesNumber: 1 },
					],
				},
			},
		],
	},
};

let cloneUser7 = clone(user7);

console.log("T9:");
console.log(JSON.stringify(user7, null, 4));
console.log(JSON.stringify(cloneUser7, null, 4));

// T10
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

let cloneStore = clone(store);

console.log("T10:");
console.log(JSON.stringify(store, null, 4));
console.log(JSON.stringify(cloneStore, null, 4));

// 2
cloneUser5.studies.department.group = 12;
cloneUser5.studies.exams.find((el) =>
	el.hasOwnProperty("programming")
).mark = 10;
console.log(JSON.stringify(cloneUser5, null, 4));

// 3
cloneUser6.studies.exams.find((el) =>
	el.hasOwnProperty("programming")
).professor.name = "Dmitry Shiman";
console.log(JSON.stringify(cloneUser6, null, 4));

// 4
cloneUser7.studies.exams
	.find((el) => el.professor.name === "Petr Ivanov")
	.professor.articles.find((el) => el.title === "About CSS").pagesNumber = 3;
console.log(JSON.stringify(cloneUser7, null, 4));

// 5
cloneStore.state.profilePage.posts.forEach((el) => (el.message = "Hello"));
console.log(JSON.stringify(cloneStore, null, 4));
