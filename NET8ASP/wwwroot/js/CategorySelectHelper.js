document.addEventListener("DOMContentLoaded", function () {
    const mainCategorySelect = document.getElementById("main-category");
    const subCategorySelect = document.getElementById("sub-category");

    mainCategorySelect.addEventListener("change", function () {
        const selectedCategoryId = this.value;

        // Очищаем список подкатегорий
        subCategorySelect.innerHTML = '<option value="" selected>Без подкатегории</option>';

        if (selectedCategoryId) {
            // Запрашиваем подкатегории через API
            fetch(`/categories/${selectedCategoryId}/subcategories`)
                .then(response => response.json())
                .then(data => {
                    data.forEach(subCategory => {
                        const option = document.createElement("option");
                        option.value = subCategory.id;
                        option.textContent = subCategory.name;
                        subCategorySelect.appendChild(option);
                    });
                })
                .catch(error => console.error("Ошибка при загрузке подкатегорий:", error));
        }
    });
});