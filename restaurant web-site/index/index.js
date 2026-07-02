//ф-я сохранения пути текущей страницы
function saveCurrentPage(page, categoryName) {
    sessionStorage.setItem('pageBefore' + page, window.location.href);
    sessionStorage.setItem('categoryName', categoryName);
}

//ф-я получения данных о категориях меню
async function getMenuCategoriesData() {
    const url = "http://localhost:8080/api/menu-categories";
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }
        
        const json = await response.json();
        addMenuCategories(json);
    } 
    catch (error) {
        console.error(error.message);
    }
}

//ф-я добавления категорий меню на страницу
function addMenuCategories(array){
    const menu = document.getElementById('menu-sections');

    //перебираем все объекты массива
    array.forEach(item => {
        //создаем новый div для каждой категории
        const menuSection = document.createElement('div');
        menuSection.classList.add('menu-section');

        //cоздаем изображение
        const img = document.createElement('img');
        img.src = 'data:image/jpg;base64,' + item.image;
        img.alt = item.categoryName;
        menuSection.appendChild(img);

        //создаем ссылку
        const link = document.createElement('a');
        link.href = `../menu-category-page/menu-category-page.html`;
        link.classList.add('menu-section-button');
        link.setAttribute('onclick', "saveCurrentPage('MenuCategoryPage', '" + item.categoryName + "')");

        //добавляем название категории внутри ссылки
        const span = document.createElement('span');
        span.textContent = item.categoryName;
        link.appendChild(span);

        //добавляем ссылку в div
        menuSection.appendChild(link);

        //добавляем div в контейнер
        menu.appendChild(menuSection);
    });
}

document.addEventListener('DOMContentLoaded', getMenuCategoriesData);