function exclude(arr1, arr2) {
	return arr1.filter((el) => !arr2.includes(el));
}

console.log(exclude(["Hi", "Hello", "Привет"], ["Привет"]));
