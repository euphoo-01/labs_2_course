class Human {
	constructor(firstName, lastName, birthYear, address) {
		this.firstName = firstName;
		this.lastName = lastName;
		this.birthYear = birthYear;
		this.address = address;
	}

	get age() {
		return new Date().getFullYear() - this.birthYear;
	}

	set age(newAge) {
		console.log(
			`Изменение возраста на ${newAge}`
		);
		this.birthYear = new Date().getFullYear() - newAge;
	}

	getFullName() {
		return `${this.firstName} ${this.lastName}`;
	}

	setAddress(newAddress) {
		console.log(`Адрес для ${this.getFullName()} изменен на: ${newAddress}`);
		this.address = newAddress;
	}

	getInfo() {
		return `${this.getFullName()}, возраст: ${this.age}, адрес: ${
			this.address
		}`;
	}
}

class Student extends Human {
	constructor(
		firstName,
		lastName,
		birthYear,
		address,
		faculty,
		course,
		group,
		studentId
	) {
		super(firstName, lastName, birthYear, address);
		this.faculty = faculty;
		this.course = course;
		this.group = group;
		this.studentId = studentId;
	}

	setCourseAndGroup(newCourse, newGroup) {
		console.log(
			`(Студент ${this.getFullName()} переведен на ${newCourse} курс, в группу ${newGroup})`
		);
		this.course = newCourse;
		this.group = newGroup;
	}

	getInfo() {
		const humanInfo = super.getInfo();
		return `${humanInfo}\nФакультет: ${this.faculty}, Курс: ${this.course}, Группа: ${this.group}, № зачетки: ${this.studentId}`;
	}
}

class Faculty {
	constructor(name, students = []) {
		this.name = name;
		this.students = students;
	}

	get studentCount() {
		return this.students.length;
	}

	get groupCount() {
		const uniqueGroups = new Set(this.students.map((s) => s.group));
		return uniqueGroups.size;
	}

	getDeviCount() {
		console.log(`\nПоиск студентов ДЭВИ на факультете "${this.name}"`);
		const deviStudents = this.students.filter((student) => {
			const specialtyCode = student.studentId.toString()[1];
			return specialtyCode === "3";
		});
		console.log(`Найдено студентов специальности ДЭВИ: ${deviStudents.length}`);
		return deviStudents;
	}

	getGroupList(groupNumber) {
		console.log(
			`\nПоиск студентов группы ${groupNumber} на факультете "${this.name}"`
		);
		const groupStudents = this.students.filter((s) => s.group === groupNumber);

		if (groupStudents.length === 0) {
			console.log(`Студенты группы ${groupNumber} не найдены.`);
			return [];
		}

		console.log(`Студенты группы ${groupNumber}:`);
		groupStudents.forEach((student) => {
			console.log(`- ${student.getFullName()}`);
		});

		return groupStudents;
	}
}

const student1 = new Student(
	"Иван",
	"Петров",
	2004,
	"ул. Ленина 10",
	"ФИТ",
	2,
	"10702122",
	"71221300"
);
const student2 = new Student(
	"Анна",
	"Сидорова",
	2003,
	"пр. Мира 5",
	"ФИТ",
	3,
	"10703221",
	"73211301"
);
const student3 = new Student(
	"Петр",
	"Иванов",
	2005,
	"ул. Советская 15",
	"ИД",
	1,
	"10601123",
	"62232302"
);
const student4 = new Student(
	"Мария",
	"Кузнецова",
	2004,
	"ул. Якуба Коласа 28",
	"ФИТ",
	2,
	"10702122",
	"73221303"
);

console.log(student1.getInfo());
student1.age = 21;
student1.setAddress("ул. Новая 1");
console.log(student1.getInfo());
console.log("\nПолное имя студента 2:", student2.getFullName());
student2.setCourseAndGroup(4, "10703320");
console.log(student2.getInfo());

const fitFaculty = new Faculty("Факультет информационных технологий", [
	student1,
	student2,
	student4,
]);

console.log(`Всего студентов: ${fitFaculty.studentCount}`);
console.log(`Всего групп: ${fitFaculty.groupCount}`);

fitFaculty.getDeviCount();
fitFaculty.getGroupList("10702122");
fitFaculty.getGroupList("999999");
