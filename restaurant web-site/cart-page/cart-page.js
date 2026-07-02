//ф-я возвращения на предыдущую страницу
function goBack() {
    const previousPage = sessionStorage.getItem('pageBeforeCart');
    if (previousPage) {
        window.location.href = previousPage;
    } 
    else {
        // Если URL не найден, возвращаем на index.html (по умолчанию)
        window.location.href = "index.html";
    }
}

//ф-я добавления позиций меню на страницу из корзины в sessionStorage
function addMenuPositions(){
    const cart = sessionStorage.getItem('cart');
    const menuPositions = JSON.parse(cart);
    menuPositions.forEach(async item => {
        try{
            const url = "http://localhost:8080/api/menu-positions/by-name/?name=" + item[0];
            const response = await fetch(url);
            if (!response.ok){
                throw new Error(`Response status: ${response.status}`);
            }
            menuPosition = await response.json();
            addMenuPosition(menuPosition, item[1]);
        }
        catch(error){
            console.error(error.message);
        }
    });
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


//ф-я добавления позиции меню на страницу 
async function addMenuPosition(menuPosition, count){
    const main = document.getElementById('main');

    //создаем обертку для элемента корзины
    const cartElement = document.createElement('div');
    cartElement.classList.add('cart-element');
    cartElement.setAttribute("id", menuPosition.name);

    //создаем обертку для позиции меню
    const menuPositionWrapper = document.createElement('div');
    menuPositionWrapper.classList.add('menu-position');

    //создаем изображение позиции меню
    const image = await getMenuPositionImagesData(menuPosition.id);
    const img = document.createElement('img');
    img.src = 'data:image/jpg;base64,' + image.image;
    img.alt = menuPosition.name;

    //создаем обертку для основной информации о позиции меню
    const menuPositionInfo = document.createElement('div');
    menuPositionInfo.classList.add('menu-position-info');

    //текстовая информация
    const textInfo = document.createElement('div');
    textInfo.classList.add('text-info');

    //название позиции меню
    const menuPositionName = document.createElement('div');
    menuPositionName.classList.add('menu-position-name');
    menuPositionName.textContent = menuPosition.name;

    //вес позиции меню
    const menuPositionWeight = document.createElement('div');
    menuPositionWeight.classList.add('menu-position-weight');
    menuPositionWeight.textContent = menuPosition.weight + " г";

    //добавляем название и вес позиции меню в div текстовой информации
    textInfo.appendChild(menuPositionName);
    textInfo.appendChild(menuPositionWeight);

    //цена позиции меню
    const menuPositionPrice = document.createElement('div');
    menuPositionPrice.classList.add('menu-position-price');
    menuPositionPrice.textContent = menuPosition.price + " ₽";

    //добавляем основную информацию о блюде в div
    menuPositionInfo.appendChild(textInfo);
    menuPositionInfo.appendChild(menuPositionPrice);

    //добавляем всю информацию о блюде в div
    menuPositionWrapper.appendChild(img);
    menuPositionWrapper.appendChild(menuPositionInfo);

    //создаем обертку для вспомогательных кнопок
    const helpfulButtons = document.createElement('div');
    helpfulButtons.classList.add('helpful-buttons');

    //изображение урны
    const span = document.createElement('span');
    span.classList.add('trash-image');
    const i = document.createElement('i');
    i.classList.add('fas', 'fa-trash');
    i.setAttribute("onclick", "deleteCartElement('" + menuPosition.name + "')");
    span.appendChild(i);

    //count-bar
    const countBar = document.createElement('div');
    countBar.classList.add('count-bar');

    //span-элементы количества блюда
    const minus = document.createElement('span');
    minus.classList.add('minus');
    minus.textContent = '−';
    minus.setAttribute("onclick", "decreaseCount()")

    const countEl = document.createElement('span');
    countEl.classList.add('count');
    countEl.textContent = count;
    countEl.setAttribute("id", "count");

    const plus = document.createElement('span');
    plus.classList.add('plus');
    plus.textContent = '+';
    plus.setAttribute("onclick", "increaseCount()")

    //добавляем span-элементы в count-bar
    countBar.appendChild(minus);
    countBar.appendChild(countEl);
    countBar.appendChild(plus);

    //добавляем вспомогательные кнопки в обертку
    helpfulButtons.appendChild(span);
    helpfulButtons.appendChild(countBar);

    //добавляем все элементы в обертку для элемента корзины
    cartElement.appendChild(menuPositionWrapper);
    cartElement.appendChild(helpfulButtons);

    main.appendChild(cartElement);
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

//ф-я удаления позиции меню из корзины 
function deleteCartElement(name){
    //удаляем элемент со страницы
    const element = document.getElementById(name);
    element.remove();

    //удаляем элемент из localStorage
    const cart = sessionStorage.getItem('cart');
    const menuPositions = JSON.parse(cart);
    for(let i = 0; i < menuPositions.length; i+=1){
        if(menuPositions[i][0] == name){
            menuPositions.splice(i, 1);
        }
    }
    sessionStorage.setItem('cart', JSON.stringify(menuPositions));
}

//ф-я очищения корзины
function clearCart(){
    //удаляем элементы корзины со страницы
    const main = document.getElementById('main');
    main.innerHTML = "";
    
    //очищаем корзину в localStorage
    menuPositions = [];
    sessionStorage.setItem('cart', JSON.stringify(menuPositions));

}

document.addEventListener('DOMContentLoaded', addMenuPositions);