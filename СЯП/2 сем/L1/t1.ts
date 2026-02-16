interface User {
	name: string;
	age: number;
}

interface Loc {
	city: string;
	country: string;
}

interface UserWithLocation extends User {
	location: Loc;
}

interface UserWithSkills extends User {
	skills: string[];
}

interface Item {
	id: number;
	name: string;
	group: number;
}

interface Exam {
	maths?: boolean;
	programming?: boolean;
	economics?: boolean;
	mark: number;
	professor?: Professor;
}

interface Professor {
	name: string;
	degree: string;
	articles?: Article[];
}

interface Article {
	title: string;
	pagesNumber: number;
}

interface Department {
	faculty: string;
	group: number;
}

interface Studies {
	university: string;
	speciality: string;
	year: number;
	exams: Exam[];
	department?: Department;
}

interface Student extends User {
	studies: Studies;
}

interface Post {
	id: number;
	message: string;
	likesCount: number;
}

interface Dialog {
	id: number;
	name: string;
}

interface Message {
	id: number;
	message: string;
}

interface StoreState {
	profilePage: {
		posts: Post[];
		newPostText: string;
	};
	dialogsPage: {
		dialogs: Dialog[];
		messages: Message[];
	};
	sidebar: any[];
}

interface Store {
	state: StoreState;
}

console.log("\n 1: \n");

// T1
let user: User = {
	name: "Masha",
	age: 21,
};

let copyUser: User = { ...user };
console.log("T1:");
console.log(copyUser);

copyUser.name = "Sasha";
console.log(copyUser);
console.log(user);

// T2
let numbers: number[] = [1, 2, 3];
let copyNumbers: number[] = [...numbers];

console.log("T2:");
console.log(numbers);
console.log(copyNumbers);
copyNumbers.push(4, 5);
console.log(numbers);
console.log(copyNumbers);

// T3
let user1: UserWithLocation = {
	name: "Masha",
	age: 23,
	location: {
		city: "Minsk",
		country: "Belarus",
	},
};
let copyUser1: UserWithLocation = { ...user1, location: { ...user1.location } };

console.log("T3:");
console.log(user1);
console.log(copyUser1);

copyUser1.location.city = "Budapest";
copyUser1.location.country = "Hungary";

console.log(user1);
console.log(copyUser1);

// T4
let user2: UserWithSkills = {
	name: "Masha",
	age: 28,
	skills: ["HTML", "CSS", "JavaScript", "React"],
};
let copyUser2: UserWithSkills = { ...user2, skills: [...user2.skills] };

console.log("T4:");
console.log(user2);
console.log(copyUser2);

copyUser2.age = 16;
copyUser2.skills.pop();
copyUser2.skills.push("Angular");

console.log(user2);
console.log(copyUser2);

// T5
const array: Item[] = [
	{ id: 1, name: "Vasya", group: 10 },
	{ id: 2, name: "Ivan", group: 11 },
	{ id: 3, name: "Masha", group: 12 },
	{ id: 4, name: "Petya", group: 10 },
	{ id: 5, name: "Kira", group: 11 },
];

const copyArray: Item[] = array.map((el) => ({ ...el }));

console.log("T5:");
console.log(array);
console.log(copyArray);

copyArray.push({ id: 6, name: "Altinbek", group: 9 });

console.log(array);
console.log(copyArray);

// T6
function clone<T>(element: T): T {
	if (typeof element !== "object" || element === null) {
		return element;
	}

	if (Array.isArray(element)) {
		return element.map((el) => clone(el)) as unknown as T;
	}

	const _clone = { ...element } as any;
	for (const key in _clone) {
		if (typeof _clone[key] === "object" && _clone[key] !== null) {
			_clone[key] = clone(_clone[key]);
		}
	}

	return _clone as T;
}

let user4: Student = {
	name: "Masha",
	age: 19,
	studies: {
		university: "BSTU",
		speciality: "designer",
		year: 2020,
		exams: [{ maths: true, mark: 8 }],
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
let user5: Student = {
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
let user6: Student = {
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
let user7: Student = {
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
let store: Store = {
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
if (cloneUser5.studies.department) {
	cloneUser5.studies.department.group = 12;
}

const progExam5 = cloneUser5.studies.exams.find((el) =>
	el.hasOwnProperty("programming"),
);
if (progExam5) {
	progExam5.mark = 10;
}

console.log("\n 2: \n");
console.log(JSON.stringify(cloneUser5, null, 4));

// 3
console.log("\n 3: \n");
const progExam6 = cloneUser6.studies.exams.find((el) =>
	el.hasOwnProperty("programming"),
);
if (progExam6 && progExam6.professor) {
	progExam6.professor.name = "Dmitry Shiman";
}
console.log(JSON.stringify(cloneUser6, null, 4));

// 4
console.log("\n 4: \n");
const targetExam7 = cloneUser7.studies.exams.find(
	(el) => el.professor?.name === "Petr Ivanov",
);
if (targetExam7?.professor?.articles) {
	const targetArticle = targetExam7.professor.articles.find(
		(el) => el.title === "About CSS",
	);
	if (targetArticle) {
		targetArticle.pagesNumber = 3;
	}
}
console.log(JSON.stringify(cloneUser7, null, 4));

// 5
console.log("\n 5: \n");
cloneStore.state.profilePage.posts.forEach((el) => (el.message = "Hello"));
console.log(JSON.stringify(cloneStore, null, 4));
