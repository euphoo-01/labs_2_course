const user = {
	name: "Josh",
	age: 18,
};

const admin = {
	accessLevel: 999,
	root: true,
	...user,
};

console.log(admin);