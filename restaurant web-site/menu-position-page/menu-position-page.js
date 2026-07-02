//ф-я возвращения на предыдущую страницу
function goBack() {
    const previousPage = sessionStorage.getItem('pageBeforeMenuPositionPage');
    if (previousPage) {
        window.location.href = previousPage;
    } 
    else {
        // Если URL не найден, возвращаем на index.html (по умолчанию)
        window.location.href = "index.html";
    }
}

//ф-я сохранения пути текущей страницы
function saveCurrentPage(page) {
    sessionStorage.setItem('pageBefore' + page, window.location.href);
}

//ф-я получения данных о позиции меню
async function getMenuPositionData(){
    const menuPositionName = sessionStorage.getItem('menuPositionName');

    //добавим данные в тег title
    document.getElementById('title').textContent = menuPositionName;
    
    try{
        const url = "http://localhost:8080/api/menu-positions/by-name/?name=" + menuPositionName;
        const response = await fetch(url);
        if (!response.ok){
            throw new Error(`Response status: ${response.status}`);
        }
        return await response.json();
    }
    catch(error){
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

        return await response.json();
    } 
    catch (error) {
        console.error(error.message);
    }
}

//ф-я добавления контента на страницу
async function addMenuPositionContent(){
    //получение данных с сервера
    const menuPosition = await getMenuPositionData();
    const menuPositionImages = await getMenuPositionImagesData(menuPosition.id);

    //добавляем фотографии позиций меню
    addMenuPositionImages(menuPosition, menuPositionImages);

    //добавляем остальную информацию о позиции меню
    addMenuPositionInfo(menuPosition);
}

//ф-я добавления фотографий позиции меню
function addMenuPositionImages(menuPosition, menuPositionImages){
    //получение основных div-ов
    const sliderWrapper = document.getElementById('slider-wrapper');
    const slider = sliderWrapper.firstElementChild;
    
    //добавляем фотографию/фотографии блюда
    for(let i = 0; i < menuPositionImages.length; i+=1){
        const img = document.createElement('img');
        img.setAttribute("id", "slide-" + (i+1));
        img.src = 'data:image/jpg;base64,' + menuPositionImages[i].image;
        img.alt = menuPosition.name;
        slider.appendChild(img);
    }

    //добавляем навигацию(если фотографий несколько)
    if(menuPositionImages.length > 1){
        const sliderNav = document.createElement('div');
        sliderNav.classList.add('slider-nav');
        for(let i = 0; i < menuPositionImages.length; i+=1){
            const link = document.createElement('a');
            link.href = "#slide-" + (i+1);
            sliderNav.appendChild(link);
        }
        sliderWrapper.appendChild(sliderNav);
    }
}

//ф-я добавления основной информации о позиции меню
function addMenuPositionInfo(menuPosition){
    //получение основных div-ов
    const menuPositionInfo = document.getElementById('menu-position-info');
    const mainInfoContent = menuPositionInfo.childNodes[7];
    const priceWithButton = menuPositionInfo.lastElementChild;
    
    //название позиции меню
    mainInfoContent.firstElementChild.textContent = menuPosition.name;

    //описание позиции меню
    mainInfoContent.childNodes[3].textContent = menuPosition.description;

    //вес позиции меню
    mainInfoContent.lastElementChild.textContent = menuPosition.weight + ' г';

    //цена позиции меню
    priceWithButton.firstElementChild.textContent = menuPosition.price + ' ₽';

    //добавляем обработчик кнопки "В корзину"
    const count = Number(document.getElementById('count').textContent);
    priceWithButton.lastElementChild.setAttribute("onclick", "addToCart('" + menuPosition.name + "', " + count + ")");
}

//ф-я добавления блюда в корзину
function addToCart(name, count){
    let cart = sessionStorage.getItem('cart');
    if (cart) {
        //если в корзине уже есть позиции меню, парсим массив из Json
        cart = JSON.parse(cart);
    } else {
        //если в корзина пуста, создаем новый массив
        cart = [];
    }

    //добавляем новую позицию меню в корзину
    cart.push([name, count]);
    sessionStorage.setItem('cart', JSON.stringify(cart));
}

//ф-я уменьшения кол-ва блюда на 1
function decreaseCount(){
    const countElement = document.getElementById('count');
    const count = countElement.textContent;
    if(count != 1){
        countElement.textContent = count - 1;
    }
    updateCount();
}

//ф-я увеличения кол-ва блюда на 1
function increaseCount(){
    const countElement = document.getElementById('count');
    const count = countElement.textContent;
    if(count != 100){
        countElement.textContent = Number(count) + 1;
    }
    updateCount();
}

//ф-я обновления обработчика кнопки "В корзину"
function updateCount(){
    const count = Number(document.getElementById('count').textContent);
    const menuPositionInfo = document.getElementById('menu-position-info');
    const priceWithButton = menuPositionInfo.lastElementChild;
    const name = menuPositionInfo.childNodes[7].firstElementChild.textContent
    priceWithButton.lastElementChild.setAttribute("onclick", "addToCart('" + name + "', " + count + ")");
}

document.addEventListener('DOMContentLoaded', addMenuPositionContent);