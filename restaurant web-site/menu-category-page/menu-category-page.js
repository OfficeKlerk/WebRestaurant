//ф-я возвращения на предыдущую страницу
function goBack() {
    const previousPage = sessionStorage.getItem('pageBeforeMenuCategoryPage');
    if (previousPage) {
        window.location.href = previousPage;
    } 
    else {
        // Если URL не найден, возвращаем на index.html (по умолчанию)
        window.location.href = "index.html";
    }
}

//ф-я получения id категории меню, на странице которой мы находимся
function getMenuName(){
    const categoryName = sessionStorage.getItem('categoryName');
    if (categoryName) {
        return getMenuCategoryId(categoryName);
    } 
    else {
        //если ключ 'categoryName' не найден, возвращаем дефолт категорию - горячие блюда
        return 'Горячие блюда';
    }
}

//ф-я получения id категории меню по имени
async function getMenuCategoryId() {
    const categoryName = sessionStorage.getItem('categoryName');
    
    //добавим на страницу данные о категории меню
    document.getElementById('title').textContent = categoryName;
    document.getElementById('menu-category-header').textContent = categoryName;

    if (categoryName){
        const url = "http://localhost:8080/api/menu-categories/by-name?name=" + categoryName;
        try {
            const response = await fetch(url);
            if (!response.ok) {
                throw new Error(`Response status: ${response.status}`);
            }
            const json = await response.json();
            return json.id;
        } 
        catch (error) {
            console.error(error.message);
        }
    }
    else{
        throw new Error('Could not get menu category name');
    }
}

//ф-я сохранения пути текущей страницы
function saveCurrentPage(page, menuPositionName) {
    sessionStorage.setItem('pageBefore' + page, window.location.href);
    sessionStorage.setItem('menuPositionName', menuPositionName)
}

//ф-я добавления блюда в корзину
function addToCart(name, event){
    event.stopPropagation();
    event.preventDefault();
    let cart = sessionStorage.getItem('cart');
    if (cart) {
        //если в корзине уже есть позиции меню, парсим массив из Json
        cart = JSON.parse(cart);
    } else {
        //если в корзина пуста, создаем новый массив
        cart = [];
    }

    //добавляем новую позицию меню в корзину
    cart.push([name, 1]);
    sessionStorage.setItem('cart', JSON.stringify(cart));
}
    

//ф-я получения данных о позициях из меню
async function getMenuPositionsData() {
    const menuCategoryId = await getMenuCategoryId();
    const url = "http://localhost:8080/api/menu-positions/by-menu-category-id/" + menuCategoryId;
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }
        
        const json = await response.json();
        await addMenuPositions(json);
    } 
    catch (error) {
        console.error(error.message);
    }
}

//ф-я получения данных об изображениях позиций меню
async function getMenuPositionImagesData(id){
    const url = "http://localhost:8080/api/menu-position-images/by-menu-position-id/" + id;
    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`Response status: ${response.status}`);
        }
        
        const json = await response.json();
        return json[0]; 
    } 
    catch (error) {
        console.error(error.message);
    }
}

//ф-я добавления позиций меню на страницу
async function addMenuPositions(array){
    const menu = document.getElementById('main');

    //перебираем все объекты массива
    array.forEach(async item => {
        //создаем карточку позиции из меню (ссылка)
        const menuPosition = document.createElement('a');
        menu.appendChild(menuPosition);
        menuPosition.classList.add('menu-position');
        menuPosition.href = "../menu-position-page/menu-position-page.html";
        menuPosition.setAttribute('onclick', "saveCurrentPage('MenuPositionPage', '" + item.name + "')");


        //cоздаем контейнер для изображения
        const imgContainer = document.createElement('div');
        imgContainer.classList.add('menu-position-image');

        //создаем изображение
        const image = await getMenuPositionImagesData(item.id);
        const img = document.createElement('img');
        img.src = 'data:image/jpg;base64,' + image.image;
        img.alt = item.name;
        imgContainer.appendChild(img);


        //создаем контейнер с информацией о позиции меню
        const menuPositionInfo = document.createElement('div');
        menuPositionInfo.classList.add('menu-position-info');

        //контейнер с названием и весом позиции меню
        const nameAndWeightContainer = document.createElement('div');
        nameAndWeightContainer.classList.add('name-with-weight');

        //название позиции меню
        const menuPositionName = document.createElement('div');
        menuPositionName.classList.add('menu-position-name');
        menuPositionName.textContent = item.name;

        //вес позиции меню
        const menuPositionWeight = document.createElement('div');
        menuPositionWeight.classList.add('menu-position-weight');
        menuPositionWeight.textContent = item.weight + " г";

        //добавляем в контейнер с названием и весом соответствующие элементы
        nameAndWeightContainer.appendChild(menuPositionName);
        nameAndWeightContainer.appendChild(menuPositionWeight);

        //контейнер с ценой и кнопкой
        const priceAndButtonContainer = document.createElement('div');
        priceAndButtonContainer.classList.add('price-with-button');

        //цена позиции меню
        const menuPositionPrice = document.createElement('div');
        menuPositionPrice.classList.add('menu-position-price');
        menuPositionPrice.textContent = item.price + " ₽";

        //контейнер кнопки добавления в корзину
        const addToCartButtonContrainer = document.createElement('div');
        addToCartButtonContrainer.classList.add('add-to-cart-button');

        //кнопка добавления в корзину
        const addToCartButton = document.createElement('button');
        addToCartButton.textContent = "В корзину";
        addToCartButton.setAttribute('onclick', "addToCart('" + item.name + "', event)");
        addToCartButtonContrainer.appendChild(addToCartButton);

        //добавляем в контейнер цены и кнопки соответствующие элементы
        priceAndButtonContainer.appendChild(menuPositionPrice);
        priceAndButtonContainer.appendChild(addToCartButtonContrainer);

        //добавляем в контейнер с информацией о позиции меню все элементы
        menuPositionInfo.appendChild(nameAndWeightContainer);
        menuPositionInfo.appendChild(priceAndButtonContainer);

        //добавляем все элементы в контейнер карточки позиции меню
        menuPosition.appendChild(imgContainer);
        menuPosition.appendChild(menuPositionInfo);
    });
}

document.addEventListener('DOMContentLoaded', getMenuPositionsData);