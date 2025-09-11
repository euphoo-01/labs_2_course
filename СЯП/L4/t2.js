const studentsSet = new Set();

const addStudent = (set, zachetka, group, fio) => {
	set.add({ zachetka, group, fio });
};

const delStudent = (set, zachetka) => {
	set.del(zachetka);
};

const filterGroup = (set, group) => {};
