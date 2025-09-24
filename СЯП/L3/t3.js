function older17(students) {
	return students.reduce((acc, student) => {
		if (student.age > 17) {
			if (!acc[student.groupId]) {
				acc[student.groupId] = [];
			}
			acc[student.groupId].push(student);
		}
		return acc;
	}, {});
}

const students = [
	{ name: "Аня", age: 13, groupId: 1 },
	{ name: "Петя Пупкин", age: 25, groupId: 1 },
	{ name: "Оля", age: 19, groupId: 2 },
	{ name: "Иван Золо", age: 17, groupId: 2 },
	{ name: "Саша", age: 20, groupId: 1 },
];

console.log(older17(students));
