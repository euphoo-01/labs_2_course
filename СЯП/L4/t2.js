const studentsSet = new Set();

const addStudent = (set, zachetka, group, fio) => {
	set.add({ zachetka, group, fio });
};

const delStudent = (set, zachetka) => {
	[...set].forEach((el) => {
		if (el.zachetka === zachetka) set.delete(el);
	});
};

const filterGroup = (set, group) => {
	return new Set([...set].filter((el) => el.group === group));
};

const sortGroup = (set) => {
	return new Set([...set].sort((el1, el2) => el1.zachetka - el2.zachetka));
};

addStudent(studentsSet, 1253333, 1, "Жмышенко Михаил Петрович");
addStudent(studentsSet, 901, 1, "Пупкин Василий Василиевич");
addStudent(studentsSet, 23526, 3, "Шиман Дмитрий Васильевич");
addStudent(studentsSet, 31616, 4, "Смелов Владимир Владиславович");

console.log(sortGroup(studentsSet));

delStudent(studentsSet, 23526);

console.log(studentsSet);

console.log(filterGroup(studentsSet, 1));
