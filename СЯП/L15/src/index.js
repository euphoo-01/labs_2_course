document.addEventListener("DOMContentLoaded", () => {
	const form = document.getElementById("validation-form");

	const fields = {
		surname: {
			element: document.getElementById("surname"),
			errorElement: document.getElementById("surname-error"),
			validators: [
				{
					validate: (value) => value.length > 0,
					message: "Поле обязательно для заполнения",
				},
				{
					validate: (value) => /^[a-zA-Zа-яА-Я]+$/.test(value),
					message: "Допустимы только буквы",
				},
				{
					validate: (value) => value.length <= 20,
					message: "Не более 20 символов",
				},
			],
		},
		name: {
			element: document.getElementById("name"),
			errorElement: document.getElementById("name-error"),
			validators: [
				{
					validate: (value) => value.length > 0,
					message: "Поле обязательно для заполнения",
				},
				{
					validate: (value) => /^[a-zA-Zа-яА-Я]+$/.test(value),
					message: "Допустимы только буквы",
				},
				{
					validate: (value) => value.length <= 20,
					message: "Не более 20 символов",
				},
			],
		},
		email: {
			element: document.getElementById("email"),
			errorElement: document.getElementById("email-error"),
			validators: [
				{
					validate: (value) => value.length > 0,
					message: "Поле обязательно для заполнения",
				},
				{
					validate: (value) => !/\s/.test(value),
					message: "Поле не должно содержать пробелы",
				},
				{
					validate: (value) => /^\S+@[a-zA-Z]{2,7}\.[a-zA-Z]{2,3}$/.test(value),
					message: "Неверный формат e-mail",
				},
			],
		},
		phone: {
			element: document.getElementById("phone"),
			errorElement: document.getElementById("phone-error"),
			validators: [
				{
					validate: (value) => value.length > 0,
					message: "Поле обязательно для заполнения",
				},
				{
					validate: (value) => /^\(0\d{2}\)\d{3}-\d{2}-\d{2}$/.test(value),
					message: "Формат телефона: (0xx)xxx-xx-xx",
				},
			],
		},
		about: {
			element: document.getElementById("about"),
			errorElement: document.getElementById("about-error"),
			validators: [
				{
					validate: (value) => value.length > 0,
					message: "Поле обязательно для заполнения",
				},
				{
					validate: (value) => value.length <= 250,
					message: "Не более 250 символов",
				},
			],
		},
	};

	form.addEventListener("submit", (e) => {
		e.preventDefault();
		let isFormValid = true;

		for (const fieldKey in fields) {
			const field = fields[fieldKey];
			const value = field.element.value.trim();
			let isFieldValid = true;
			let errorMessage = "";

			for (const validator of field.validators) {
				if (!validator.validate(value)) {
					isFieldValid = false;
					isFormValid = false;
					errorMessage = validator.message;
					break;
				}
			}

			if (!isFieldValid) {
				field.errorElement.textContent = errorMessage;
				field.element.classList.add("invalid");
			} else {
				field.errorElement.textContent = "";
				field.element.classList.remove("invalid");
			}
		}

		if (isFormValid) {
			alert("Форма успешно отправлена!");
			form.reset();
		}
	});
});
